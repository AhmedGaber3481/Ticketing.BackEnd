using LinkDev.Ticketing.Core.Models;
using LinkDev.UserManagent.Application.Interfaces;
using LinkDev.UserManagent.Domain.DTOs;
using LinkDev.UserManagent.WebAPI.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace LinkDev.UserManagent.WebAPI.Controllers
{
    [Route("api/Users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly LinkDev.Ticketing.Logging.Application.Interfaces.ILogger _logger;
        private readonly IUserManager _userManager;

        public UsersController(LinkDev.Ticketing.Logging.Application.Interfaces.ILogger logger, IUserManager userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        [Route("GetUsers"), HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] UserSearchDTO requestDTO)
        {
            Guid correlationId = Guid.NewGuid();

            _logger.LogInformation("GetUsers Filtered", "UsersController", "GetUsers", correlationId, id1: requestDTO.SearchValue);

            try
            {
                //var users = new ListViewResult<UserListDTO>();
                //users.Items = new List<UserListDTO>()
                //{
                //    new UserListDTO()
                //    {
                //        UserName="Admin",
                //        UserFullName = "Adminstrator",
                //        Email ="test@yahoo.com",
                //        PhoneNumber ="1223445",
                //        UserId = Guid.NewGuid().ToString(),
                //        UserRole ="Admin"
                //    },
                //    new UserListDTO()
                //    {
                //        UserName="Ahmed",
                //        UserFullName = "Adminstrator2",
                //        Email ="test2@yahoo.com",
                //        PhoneNumber ="1223446",
                //        UserId = Guid.NewGuid().ToString(),
                //        UserRole ="Admin"
                //    }
                //};

                var users = _userManager.GetUsersList(requestDTO, correlationId);

                if(users == null)
                {
                    return ResponseMessageHelper.ServerError(correlationId);
                }

                return ResponseMessageHelper.Ok(users);
            }
            catch (Exception exp)
            {
                _logger.LogError(exp, "Exception in GetUsers", "UsersController", "GetUsers", correlationId);

                return ResponseMessageHelper.ServerError(correlationId);
            }
        }
    }
}
