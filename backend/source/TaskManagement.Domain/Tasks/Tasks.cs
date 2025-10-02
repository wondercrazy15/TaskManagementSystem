using TaskManagement.Domain.Tasks.AssignTasks;

namespace TaskManagement.Domain.Tasks
{
    /// <summary>
    /// Tasks.
    /// </summary>
    public class Tasks
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int TaskStatusId { get; set; }
        public string Priority { get; set; } = string.Empty;
        public Guid CreatorId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<AssignUsersToTask> TaskAssignments { get; set; } = new List<AssignUsersToTask>();

    }
}







