using LinkDev.UserManagent.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace LinkDev.UserManagent.Application.Services
{
    public class LoggedUserService : ILoggedUserService
    {
        private readonly IUserManager _loggedUserRepository;
        public LoggedUserService(IUserManager loggedUserRepository)
        {
            _loggedUserRepository = loggedUserRepository;
        }
        public async Task<string> GetLoggedUserId()
        {
            return await _loggedUserRepository.GetLoggedUserId();
        }
    }
}
