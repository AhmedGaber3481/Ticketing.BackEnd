using System.ComponentModel.DataAnnotations;

namespace LinkDev.UserManagent.Domain.Models
{
    public class RegisteredUser : User
    {
        [Required]
        public string Email { get; set; }
    }
}
