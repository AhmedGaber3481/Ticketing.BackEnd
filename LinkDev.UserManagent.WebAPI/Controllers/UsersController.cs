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

        [Route("GetUser/{id}"), HttpGet]
        public async Task<IActionResult> GetUsers(string id)
        {
            Guid correlationId = Guid.NewGuid();

            _logger.LogInformation("GetUser Id", "UsersController", "GetUser", correlationId, id1: id);

            try
            {
                var users = _userManager.GetUserById(id, correlationId);

                if (users == null)
                {
                    return ResponseMessageHelper.ServerError(correlationId);
                }

                return ResponseMessageHelper.Ok(users);
            }
            catch (Exception exp)
            {
                _logger.LogError(exp, "Exception in GetUser", "UsersController", "GetUser", correlationId);

                return ResponseMessageHelper.ServerError(correlationId);
            }
        }
    }
}
