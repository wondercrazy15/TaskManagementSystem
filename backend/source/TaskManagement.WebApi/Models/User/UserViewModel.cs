namespace TaskManagementWebApi.Models.User
{
    /// <summary>
    /// User View Model
    /// </summary>
    public class UserViewModel
    {
        /// <summary>
        /// User Identifire
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; } = String.Empty;

        ///// <summary>
        ///// Phone Number
        /// </summary>
        //public string PhoneNumber { get; set; } = String.Empty;

        ///// <summary>
        ///// First Name
        ///// </summary>
        //public string FirstName { get; set; } = String.Empty;

        ///// <summary>
        ///// Last Name
        ///// </summary>
        //public string LastName { get; set; } = String.Empty;

        ///// <summary>
        ///// Date Of Birth
        ///// </summary>
        //public string DateOfBirth { get; set; } = String.Empty;

        ///// <summary>
        ///// Profile Image
        ///// </summary>
        //public string? ProfileImage { get; set; }


        /// <summary>
        /// Role
        /// </summary>
        public string Role { get; set; }


        /// <summary>
        /// Created date
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }

    /// <summary>
    /// User task detail view model
    /// </summary>
    public class UserTaskDetailViewModel
    {
        /// <summary>
        /// User Identifire
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// User name
        /// </summary>
        public string Username { get; set; } = String.Empty;
    }
}








