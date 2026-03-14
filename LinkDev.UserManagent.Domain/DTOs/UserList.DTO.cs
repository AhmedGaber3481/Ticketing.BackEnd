using System;
using System.Collections.Generic;
using System.Text;

namespace LinkDev.UserManagent.Domain.DTOs
{
    public class UserListDTO
    {
        public string? UserFullName { get; set; }
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? UserRole { get; set; }
    }
}
