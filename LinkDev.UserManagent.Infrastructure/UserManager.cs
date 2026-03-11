using LinkDev.UserManagent.Application.Interfaces;
using LinkDev.UserManagent.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace LinkDev.UserManagent.Infrastructure.Repositories
{
    public class UserManager : IUserManager
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _contextAccessor;

        public UserManager(UserManager<IdentityUser> userManager,
            IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
            _userManager = userManager;
        }
        public async Task<string> GetLoggedUserId()
        {
            string userId = string.Empty;
            var identityUser = _contextAccessor.HttpContext?.User?.Identity;
            if (identityUser != null && identityUser.IsAuthenticated)
            {
                IdentityUser? user = await _userManager.FindByNameAsync(identityUser.Name!);
                userId = user?.Id ?? string.Empty;
            }

            return userId;
        }

        public bool IsInRole(string roleName)
        {
            var user = _contextAccessor.HttpContext?.User;
            return user != null && user.IsInRole(roleName);
        }
    }
}
