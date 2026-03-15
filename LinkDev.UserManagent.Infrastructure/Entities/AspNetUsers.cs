using Microsoft.AspNetCore.Identity;

namespace LinkDev.UserManagent.Infrastructure.Entities
{
    public class AspNetUsers : IdentityUser
    {
        public UserDetails? UserDetails { get; set; }
    }
}
