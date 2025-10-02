using System.ComponentModel.DataAnnotations;

namespace TaskManagementWebApi.Models.User
{
    /// <summary>
    /// Login Model
    /// </summary>
    public class LoginModel
    {
        /// <summary>
        /// Email
        /// </summary>
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage ="Invalid email address")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Password
        /// </summary>
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty;
    }
}







