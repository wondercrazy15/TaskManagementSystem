using TaskManagement.Domain.UserDetails;

namespace TaskManagement.Domain.UserDetail
{
    public interface IUsersService
    {
        /// <summary>
        /// Add User.
        /// </summary>
        /// <returns>User.</returns>
        Task<User> AddUserAsync(User user);

        /// <summary>
        /// Get Users.
        /// </summary>
        /// <returns>Users.</returns>
        Task<List<User>> GetUsersAsync();
        Task<User?> GetUserByIdAsync(Guid id);
    }
}







