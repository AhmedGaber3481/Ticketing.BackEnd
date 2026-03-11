using System;
using System.Collections.Generic;
using System.Text;

namespace LinkDev.UserManagent.Application.Interfaces
{
    public interface IUserManager
    {
        Task<string> GetLoggedUserId();
        bool IsInRole(string roleName);
    }
}
