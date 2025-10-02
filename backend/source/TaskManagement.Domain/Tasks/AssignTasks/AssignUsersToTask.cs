using TaskManagement.Domain.UserDetails;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagement.Domain.Tasks.AssignTasks
{
    /// <summary>
    /// Assign users to task.
    /// </summary>
    public class AssignUsersToTask
    {
        public int Id { get; set; }

        public Guid TaskId { get; set; }
        [ForeignKey(nameof(TaskId))]
        public virtual Tasks Tasks { get; set; } = null!;

        public Guid AssigneeId { get; set; }
        [ForeignKey(nameof(AssigneeId))]
        public virtual User Users { get; set; } = null!;
    }
}







