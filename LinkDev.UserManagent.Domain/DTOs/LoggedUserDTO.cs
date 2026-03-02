using System;
using System.Collections.Generic;
using System.Text;

namespace LinkDev.UserManagent.Domain.DTOs
{
    public class LoggedUserDTO
    {
        public string? UserFullName { get; set; }

        public required string UserId { get; set; }
    }
}
