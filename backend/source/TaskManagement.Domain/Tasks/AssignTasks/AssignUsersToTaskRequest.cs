namespace TaskManagement.Domain.Tasks.AssignTasks
{
    /// <summary>
    /// Assign users to task request.
    /// </summary>
    public class AssignUsersToTaskRequest
    {
        public Guid TaskId { get; set; }
        public List<Guid> UserIds { get; set; } = new List<Guid>(); 
    }
}







