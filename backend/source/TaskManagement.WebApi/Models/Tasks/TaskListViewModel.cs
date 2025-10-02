namespace TaskManagementWebApi.Models.Tasks
{
    /// <summary>
    /// GetAll task view model.
    /// </summary>
    public class TaskListViewModel
    {
        /// <summary>
        /// Id of the task.
        /// </summary>
        public Guid Id { get; set; }


        /// <summary>
        /// Title of the task.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Description of the task.    
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Task status id.
        /// </summary>
        public int TaskStatusId { get; set; }

        /// <summary>
        /// Priority of the task.
        /// </summary>
        public string Priority { get; set; } = string.Empty;


        /// <summary>
        /// Creator Id of the task.
        /// </summary>
        public Guid CreatorId { get; set; }

        /// <summary>
        /// Creation date of the task. 
        /// </summary>
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// Update date of the task.    
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// User Ids assigned to the task.
        /// </summary>
        public List<Guid>? TaskAssignments { get; set; }
    }
}







