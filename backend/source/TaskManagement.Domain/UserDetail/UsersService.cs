using TaskManagement.Domain.UserDetails;

namespace TaskManagement.Domain.UserDetail
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository usersRepository;

        /// <summary>
        /// Creates service.
        /// </summary>
        public UsersService(IUsersRepository _usersRepository)
        {
            usersRepository = _usersRepository;
        }

        public async Task<User> AddUserAsync(User user)
        {
            return await usersRepository.AddUserAsync(user);
        }

        public Task<User?> GetUserByIdAsync(Guid id)
        {
            return usersRepository.GetUserByIdAsync(id);
        }
        public async Task<List<User>> GetUsersAsync()
        {
            return await usersRepository.GetUsersAsync();
        }
    }
}







