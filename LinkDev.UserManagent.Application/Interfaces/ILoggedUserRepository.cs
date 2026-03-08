using System;
using System.Collections.Generic;
using System.Text;

namespace LinkDev.UserManagent.Application.Interfaces
{
    public interface ILoggedUserRepository
    {
        Task<string> GetLoggedUserId();
    }
}
