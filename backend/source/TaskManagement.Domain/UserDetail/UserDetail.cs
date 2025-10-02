using TaskManagement.Domain.Tasks.AssignTasks;

namespace TaskManagement.Domain.UserDetails
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;    
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public virtual ICollection<AssignUsersToTask> AssignedTasks { get; set; } = new List<AssignUsersToTask>();

    }
}







