namespace TaskManagementWebApi.Models.User
{
    /// <summary>
    /// Login View Model
    /// </summary>
    public class LoginViewModel : UserViewModel
    {
        /// <summary>
        /// token
        /// </summary>
        public string token { get; set; } = String.Empty;

        /// <summary>
        /// Expiration
        /// </summary>
        public DateTime expiration { get; set; }
    }
}







