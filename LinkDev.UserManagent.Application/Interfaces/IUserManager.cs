using LinkDev.Ticketing.Core.Models;
using LinkDev.UserManagent.Domain.DTOs;

namespace LinkDev.UserManagent.Application.Interfaces
{
    public interface IUserManager
    {
        Task<string> GetLoggedUserId();
        bool IsInRole(string roleName);
        ListViewResult<UserDTO>? GetUsersList(UserSearchDTO requestDTO, Guid correlationId);
        UserDTO? GetUserById(string userId, Guid correlationId);
    }
}
