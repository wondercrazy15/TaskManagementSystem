
using TaskManagement.Domain.UserDetails;

namespace TaskManagement.Domain.UserDetail
{
    public interface IUsersRepository
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

        /// <summary>
        /// Get user by identifier.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<User?> GetUserByIdAsync(Guid id);
   
    }
}







