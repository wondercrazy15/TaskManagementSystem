using TaskManagement.Domain.Tasks.AssignTasks;

namespace TaskManagement.Domain.Tasks
{
    public class TaskService : ITaskService
    {
        private readonly ITasksRepository _taskRepository;

        public TaskService(ITasksRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<Tasks> GetByIdAsync(Guid id)
        {
            return await _taskRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Tasks>> GetAllAsync(int? taskStatusId = null, Guid? assigneeId = null)
        {
            return await _taskRepository.GetAllAsync(taskStatusId, assigneeId);
        }
        public async Task<IEnumerable<Tasks>> GetAllAssignnesAsync(List<Guid> assigneeId)
        {
            return await _taskRepository.GetAllAssignnesAsync(assigneeId);
        }

        public async Task<Tasks> AddAsync(Tasks task)
        {
            return await _taskRepository.AddAsync(task);
        }

        public async Task<Tasks> UpdateAsync(Tasks task)
        {
            return await _taskRepository.UpdateAsync(task);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _taskRepository.DeleteAsync(id);
        }

        public async Task<bool> AssignTaskAsync(AssignUsersToTaskRequest assignUsersToTaskRequest)
        {
            return await _taskRepository.AssignTaskAsync(assignUsersToTaskRequest);
        }
    }
}







