using System.ComponentModel.DataAnnotations;

namespace LinkDev.UserManagent.Domain.Models
{
    public class ChangePassword
    {
        [Required]
        public string? UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string NewPassword { get; set; }

        [Required, Compare("NewPassword", ErrorMessage = "New Password and Confirm Password should be the same.")]
        public string PasswordConfirm { get; set; }
    }
}
