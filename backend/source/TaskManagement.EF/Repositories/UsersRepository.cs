using TaskManagement.Domain.UserDetail;
using TaskManagement.Domain.UserDetails;
using Microsoft.EntityFrameworkCore;

namespace TaskManagement.EF.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private TaskManagementDbContext dbContext;
        /// <summary>
        /// Creates repository.
        /// </summary>
        /// <param name="context">Context.</param>
        public UsersRepository(TaskManagementDbContext context)
        {
            dbContext = context;
        }

        public async Task<User> AddUserAsync(User user)
        {
            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();
            return user;
        }

        public async Task<List<User>> GetUsersAsync()
        {
            return await GetQuery().ToListAsync();
        }

        public async Task<User?> GetUserByIdAsync(Guid id)
        {
            return await GetQuery().Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            dbContext.Update(user);
            await dbContext.SaveChangesAsync();
            return null;
        }

        private IQueryable<User> GetQuery()
        {
            var query = dbContext.Users.AsNoTracking();
            return query;
        }
    }
}







