namespace TaskManagementWebApi.Models.Tasks
{
    /// <summary>
    /// Update task view Model.
    /// </summary>
    public class UpdateTaskViewModel
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
    }
}







