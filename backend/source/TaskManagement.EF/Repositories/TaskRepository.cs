using TaskManagement.Domain.Tasks;
using TaskManagement.Domain.Tasks.AssignTasks;
using Microsoft.EntityFrameworkCore;

namespace TaskManagement.EF.Repositories
{
    public class TaskRepository : ITasksRepository
    {
        private readonly TaskManagementDbContext dbContext;

        /// <summary>
        /// Creates repository.
        /// </summary>
        /// <param name="context">Context.</param>
        public TaskRepository(TaskManagementDbContext context)
        {
            dbContext = context;
        }

        public async Task<Tasks> GetByIdAsync(Guid id)
        {
            return await dbContext.Tasks.FindAsync(id) ?? null!;
        }

        public async Task<IEnumerable<Tasks>> GetAllAsync(int? taskStatusId = null, Guid? assigneeId = null)
        { 
            IQueryable<Tasks> query = dbContext.Tasks.Include(x=>x.TaskAssignments);
            if (taskStatusId.HasValue)
                query = query.Where(t => t.TaskStatusId == taskStatusId.Value);

            if (assigneeId.HasValue)
                query = query.Where(t => t.TaskAssignments.Any(au => au.AssigneeId == assigneeId.Value));

            return await query.ToListAsync();
        }
        public async Task<IEnumerable<Tasks>> GetAllAssignnesAsync(List<Guid> assigneeIds)
        {
            IQueryable<Tasks> query = dbContext.Tasks.Include(x => x.TaskAssignments);
          
            if(assigneeIds != null && assigneeIds.Count > 0)
                query = query.Where(t => t.TaskAssignments.Where(a=> assigneeIds.Contains(a.AssigneeId)).Any());

            return await query.ToListAsync();
        }

        public async Task<Tasks> AddAsync(Tasks task)
        {
            task.Id = Guid.NewGuid();
            task.CreatedAt = DateTime.UtcNow;
            dbContext.Tasks.Add(task);
            await dbContext.SaveChangesAsync();
            return task;
        }

        public async Task<Tasks> UpdateAsync(Tasks task)
        {
            dbContext.Tasks.Update(task);
            await dbContext.SaveChangesAsync();
            return task;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var task = await dbContext.Tasks.FindAsync(id);
            var assignments = dbContext.AssignUsersToTasks.Where(a => a.TaskId == id).ToList(); 
            if (task == null)
                return false;

            if (assignments.Any())
                dbContext.AssignUsersToTasks.RemoveRange(assignments);

            dbContext.Tasks.Remove(task);
            
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AssignTaskAsync(AssignUsersToTaskRequest assignUsersToTaskRequest)
        {
            var task = await GetByIdAsync(assignUsersToTaskRequest.TaskId);
            if (task == null)
                return false;

            var existingAssignments = dbContext.AssignUsersToTasks
                .Where(a => a.TaskId == assignUsersToTaskRequest.TaskId)
                .ToList();

            var userIdsToAssign = assignUsersToTaskRequest.UserIds;
            var assignmentsToRemove = existingAssignments
                .Where(a => !userIdsToAssign.Contains(a.AssigneeId))
                .ToList();

            if (assignmentsToRemove.Any())
                dbContext.AssignUsersToTasks.RemoveRange(assignmentsToRemove);

            var existingUserIds = existingAssignments.Select(a => a.AssigneeId).ToList();
            var newAssignments = userIdsToAssign
                .Where(id => !existingUserIds.Contains(id))
                .Select(id => new AssignUsersToTask
                {
                    TaskId = assignUsersToTaskRequest.TaskId,
                    AssigneeId = id
                }).ToList();

            if (newAssignments.Any())
                await dbContext.AssignUsersToTasks.AddRangeAsync(newAssignments);

            await dbContext.SaveChangesAsync();

            return true;
        }

    }
}







