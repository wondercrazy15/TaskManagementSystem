namespace TaskManagementWebApi.Models.User
{
    /// <summary>
    /// User Detail View Model
    /// </summary>
    public class UserDetailViewModel : UserViewModel
    {
        /// <summary>
        /// Email Confirmed
        /// </summary>
        public bool? EmailConfirmed { get; set; }

        /// <summary>
        /// Phone Number Confirmed
        /// </summary>
        public bool? PhoneNumberConfirmed { get; set; }
    }
}







