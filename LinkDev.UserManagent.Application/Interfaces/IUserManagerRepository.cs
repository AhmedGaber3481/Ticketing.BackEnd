using LinkDev.UserManagent.Domain.DTOs;
using LinkDev.UserManagent.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LinkDev.UserManagent.Application.Interfaces
{
    public interface IUserManagerRepository
    {
        Task<ResponseMessage<LoginResultDTO>> SignIn(User loggedUser);

        Task<ResponseMessage<LoggedUserDTO>> GetUserDetails(string userName);
        Task<string> GetLoggedUserId();
    }
}
