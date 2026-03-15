using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace LinkDev.UserManagent.Domain.Entities
{
    //public class AspNetUsers
    //{
    //    public required string Id { get; set; }
    //    public string? Email { get; set; }
    //    public bool EmailConfirmed { get; set; }
    //    public string? PasswordHash { get; set; }
    //    public string? SecurityStamp { get; set; }
    //    public string? PhoneNumber { get; set; }
    //    public bool PhoneNumberConfirmed { get; set; }
    //    public bool TwoFactorEnabled { get; set; }
    //    public DateTimeOffset? LockoutEndDateUtc { get; set; }
    //    public bool LockoutEnabled { get; set; }
    //    public int AccessFailedCount { get; set; }
    //    public required string UserName { get; set; }
    //    public required string NormalizedUserName { get; set; }
    //    public string? ConcurrencyStamp { get; set; }
    //    public DateTimeOffset? LockoutEnd { get; set; }
    //    public string? NormalizedEmail { get; set; }
    //    public ICollection<AspNetUserRoles>? UserRoles { get; set; }
    //    public UserDetails? UserDetails { get; set; }
    //}

    public class AspNetUsers : IdentityUser
    {
        public UserDetails? UserDetails { get; set; }

        //public ICollection<AspNetUserRoles> UserRoles { get; set; }
    }
}
