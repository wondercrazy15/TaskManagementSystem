using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace TaskManagement.EF.Services
{
    /// <summary>
    /// Database migration service.
    /// </summary>
    public class TaskManagementDbMigrationService : IDbMigrationService
    {
        private readonly IConfigurationRoot _configurationRoot;
        private readonly TaskManagementDbContext _context;

        /// <summary>
        /// Creates service.
        /// </summary>
        /// <param name="configuration">Configuration root.</param>
        /// <param name="context">Database context.</param>
        public TaskManagementDbMigrationService(
            IConfiguration configuration,
            TaskManagementDbContext context)
        {
            _configurationRoot = (IConfigurationRoot)configuration;
            _context = context;
        }

        /// <summary>
        /// Migrates database.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task MigrateAsync()
        {
            await _context.Database.MigrateAsync();
            _configurationRoot.Reload();
        }
    }
}







