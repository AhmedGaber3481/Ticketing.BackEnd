using LinkDev.UserManagent.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace LinkDev.UserManagent.Application.Services
{
    public class LoggedUserService : ILoggedUserService
    {
        private readonly ILoggedUserRepository _loggedUserRepository;
        public LoggedUserService(ILoggedUserRepository loggedUserRepository)
        {
            _loggedUserRepository = loggedUserRepository;
        }
        public async Task<string> GetLoggedUserId()
        {
            return await _loggedUserRepository.GetLoggedUserId();
        }
    }
}
