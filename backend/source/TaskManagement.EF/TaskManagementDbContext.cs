using TaskManagement.Domain.Tasks;
using TaskManagement.Domain.Tasks.AssignTasks;
using TaskManagement.Domain.UserDetails;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TaskManagement.EF
{
    public class TaskManagementDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public TaskManagementDbContext(DbContextOptions<TaskManagementDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Domain.Tasks.TaskStatus>().HasData(
                new Domain.Tasks.TaskStatus { Id = 1, Name = "Todo" },
                new Domain.Tasks.TaskStatus { Id = 2, Name = "InProgress" },
                new Domain.Tasks.TaskStatus { Id = 3, Name = "Done" }
            );
        }
       
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Tasks> Tasks { get; set; }

        public virtual DbSet<Domain.Tasks.TaskStatus> TaskStatuses { get; set; }

        public virtual DbSet<AssignUsersToTask> AssignUsersToTasks { get; set; }
    }
}







