using System;
using System.Collections.Generic;
using System.Text;

namespace LinkDev.UserManagent.Domain.Entities
{
    public class UserDetails
    {
        public required string UserId { get; set; }
        public string? FullName { get; set; }
    }
}
