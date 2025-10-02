using TaskManagement.Domain.UserDetail;
using TaskManagement.Domain.UserDetails;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;

namespace TaskManagement.EF.DataConfigurations
{
    public static class SeedAdmin
    {
        public static async Task SeedAdminAsync(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var usersService = services.GetRequiredService<IUsersService>();

            var adminEmail = "admin@gmail.com";
            var adminPassword = "Admin@123";
            var adminRole = "Admin";

            // 1. Create role if not exists
            if (!await roleManager.RoleExistsAsync(adminRole))
            {
                await roleManager.CreateAsync(new IdentityRole(adminRole));
            }

            // 2. Create IdentityUser if not exists
            var identityAdmin = await userManager.FindByEmailAsync(adminEmail);
            if (identityAdmin == null)
            {
                identityAdmin = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(identityAdmin, adminPassword);
                if (!result.Succeeded)
                    throw new Exception($"Failed to create admin user: {string.Join(", ", result.Errors.Select(e => e.Description))}");

                // Assign role
                await userManager.AddToRoleAsync(identityAdmin, adminRole);
            }

            // 3. Create custom User table entry if not exists
            var customAdmin = await usersService.GetUserByIdAsync(Guid.Parse(identityAdmin.Id));
            if (customAdmin == null)
            {
                await usersService.AddUserAsync(new User
                {
                    Id = Guid.Parse(identityAdmin.Id), // map IdentityUser.Id
                    Username = "admin",
                    Email = adminEmail,
                    Role = adminRole,
                    CreatedAt = DateTime.UtcNow
                });
            }
        }

    }
}







