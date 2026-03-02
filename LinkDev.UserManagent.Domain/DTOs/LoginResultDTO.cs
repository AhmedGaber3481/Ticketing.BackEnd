using System;
using System.Collections.Generic;
using System.Text;

namespace LinkDev.UserManagent.Domain.DTOs
{
    public class LoginResultDTO
    {
        public bool LoginSuccess { get; set; }
        public string? UserFullName { get; set; }
        public string? UserId { get; set; }
    }
}
