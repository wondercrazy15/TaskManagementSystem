using TaskManagement.Domain.Tasks;
using TaskManagement.EF;
using TaskManagement.EF.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace TaskManagement.Tests
{
    public class TaskServiceTests
    {
        private readonly TaskService _service;
        private readonly TaskManagementDbContext _context;

        public TaskServiceTests()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = config.GetConnectionString("TaskManagementConnectionString");
            var options = new DbContextOptionsBuilder<TaskManagementDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            _context = new TaskManagementDbContext(options);
            _context.Database.EnsureCreated();
            var repository = new TaskRepository(_context);
            _service = new TaskService(repository);
        }
        [Fact]
        public async Task AddTask_ShouldSaveToDatabase()
        {
            var task = new Tasks { Id = Guid.NewGuid(), Title = "Test Task" };
            await _service.AddAsync(task);
            var savedTask = await _service.GetByIdAsync(task.Id);
            Assert.NotNull(savedTask);
            Assert.Equal("Test Task", savedTask.Title);
        }
        [Fact]
        public async Task GetTask_ShouldReturnTask_WhenExists()
        {
            var task = new Tasks { Id = Guid.NewGuid(), Title = "Another Task" };
            await _service.AddAsync(task);
            var fetchedTask = await _service.GetByIdAsync(task.Id);
            Assert.NotNull(fetchedTask);
            Assert.Equal(task.Title, fetchedTask!.Title);
        }
    }
}






