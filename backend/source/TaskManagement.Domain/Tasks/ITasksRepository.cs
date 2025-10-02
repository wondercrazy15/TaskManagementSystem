using TaskManagement.Domain.Tasks.AssignTasks;

namespace TaskManagement.Domain.Tasks
{
    public interface ITasksRepository
    {
        Task<Tasks> GetByIdAsync(Guid id);
        Task<IEnumerable<Tasks>> GetAllAsync(int? taskStatusId = null, Guid? assigneeId = null);

        Task<IEnumerable<Tasks>> GetAllAssignnesAsync(List<Guid> assigneeId);
        Task<Tasks> AddAsync(Tasks task);
        Task<bool> AssignTaskAsync(AssignUsersToTaskRequest assignUsersToTaskRequest);
        Task<Tasks> UpdateAsync(Tasks task);
        Task<bool> DeleteAsync(Guid id);
    }
}







