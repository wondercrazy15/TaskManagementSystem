using System.ComponentModel.DataAnnotations;

namespace TaskManagementWebApi.Models.User
{
    /// <summary>
    /// Register Model
    /// </summary>
    public class RegisterModel
    {
        /// <summary>
        /// UserName
        /// </summary>
        [Required(ErrorMessage = "UserName is required")]
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// Email
        /// </summary>
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Password
        /// </summary>
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Role
        /// </summary>
        [Required(ErrorMessage = "Role is required")]
        public string Role { get; set; } = string.Empty;

        /// <summary>
        /// CreatedAt
        /// </summary>
        [Required(ErrorMessage = "CreatedAt is required")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}







