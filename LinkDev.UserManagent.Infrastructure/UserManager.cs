using LinkDev.Ticketing.Core.Models;
using LinkDev.UserManagent.Application.Interfaces;
using LinkDev.UserManagent.Domain.DTOs;
using LinkDev.UserManagent.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace LinkDev.UserManagent.Infrastructure.Repositories
{
    public class UserManager : IUserManager
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly LinkDev.Ticketing.Logging.Application.Interfaces.ILogger _logger;

        public UserManager(UserManager<IdentityUser> userManager,
            IHttpContextAccessor contextAccessor,
            ApplicationDbContext applicationDbContext,
            LinkDev.Ticketing.Logging.Application.Interfaces.ILogger logger)
        {
            _contextAccessor = contextAccessor;
            _userManager = userManager;
            _applicationDbContext = applicationDbContext;
            _logger = logger;
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

        public ListViewResult<UserListDTO>? GetUsersList(UserSearchDTO requestDTO, Guid correlationId)
        {
            try
            {
                ListViewResult<UserListDTO> listViewResult = new ListViewResult<UserListDTO>();

                if (requestDTO.PageNumber < 1)
                {
                    requestDTO.PageNumber = 1;
                }

                var query =
                from user in _applicationDbContext.Users
                join userDetails in _applicationDbContext.UserDetails
                    on user.Id equals userDetails.UserId
                join userRole in _applicationDbContext.UserRoles
                    on user.Id equals userRole.UserId
                join role in _applicationDbContext.Roles
                    on userRole.RoleId equals role.Id
                select new { user, userDetails, role };

                if (!string.IsNullOrWhiteSpace(requestDTO.SearchValue))
                {
                    query = query.Where(x => x.user.UserName != null &&
                                             x.user.UserName.Contains(requestDTO.SearchValue));
                }

                var result = query
                    .OrderBy(x => x.user.UserName)
                    .Skip((requestDTO.PageNumber - 1) * requestDTO.PageSize)
                    .Take(requestDTO.PageSize)
                    .ToList();


                var queryCount = _applicationDbContext.Users.AsQueryable();
                if (!string.IsNullOrWhiteSpace(requestDTO.SearchValue))
                {
                    queryCount = queryCount.Where(x => x.UserName != null && x.UserName.Contains(requestDTO.SearchValue));
                }

                listViewResult.TotalCount = queryCount.Count();


                listViewResult.Items = query.Select(x => new UserListDTO()
                {
                    Email = x.user.Email,
                    PhoneNumber = x.user.PhoneNumber,
                    UserFullName = x.userDetails.FullName,
                    UserId = x.user.Id,
                    UserName = x.user.UserName,
                    UserRole = x.role.Name
                }).ToList();
                listViewResult.PageSize = requestDTO.PageSize;
                listViewResult.PageNumber = requestDTO.PageNumber;

                return listViewResult;
            }
            catch (Exception exp)
            {
                _logger.LogError(exp, "Exception in GetUsersList", "UserManager", "GetUsersList", correlationId);

                return null;
            }
        }

        public bool IsInRole(string roleName)
        {
            var user = _contextAccessor.HttpContext?.User;
            return user != null && user.IsInRole(roleName);
        }
    }
}
