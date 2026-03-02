using LinkDev.UserManagent.Application.Interfaces;
using LinkDev.UserManagent.Domain.DTOs;
using LinkDev.UserManagent.Domain.Models;
using LinkDev.UserManagent.WebAPI.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Identity.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly LinkDev.Ticketing.Logging.Application.Interfaces.ILogger _logger;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IUserManagerRepository _userManagerRepository;

        public AccountController(SignInManager<IdentityUser> signInManager,
            LinkDev.Ticketing.Logging.Application.Interfaces.ILogger logger,
            IHttpContextAccessor contextAccessor,
            IUserManagerRepository userManagerRepository)
        {
            _signInManager = signInManager;
            _logger = logger;
            _contextAccessor = contextAccessor;
            _userManagerRepository = userManagerRepository;
        }


        [Route("SignIn"), HttpPost]
        public async Task<IActionResult> SignIn([FromBody]User loggedUser)
        {
            Guid correlationId = Guid.NewGuid();
            
            try
            {
                ResponseMessage<LoginResultDTO>? response = null;
                if (ModelState.IsValid)
                {
                    response = await _userManagerRepository.SignIn(loggedUser);
                }
                else
                {
                    response = new ResponseMessage<LoginResultDTO>();
                    response.Notifications = ErrorMessageHelper.GetErrorMessages(ModelState);
                }
                return ResponseMessageHelper.GetResult(response);

            }
            catch (Exception exp)
            {
                _logger.LogError(exp, "Exception in SignIn", "AccountController", "SignIn", correlationId);
                return ResponseMessageHelper.ServerError(correlationId);
            }
        }

        [Route("SignOut"), HttpGet]
        new public async Task<IActionResult> SignOut()
        {
            Guid correlationId = Guid.NewGuid();
            try
            {
                await _signInManager.SignOutAsync();
                return ResponseMessageHelper.Ok(new ResponseMessage<bool>() { Data = true });
            }
            catch (Exception exp)
            {
                _logger.LogError(exp, "Exception in SignOut", "AccountController", "SignOut", correlationId);
                return ResponseMessageHelper.ServerError(correlationId);
            }
        }

        [Route("GetLoggedUser"), HttpGet]
        public async Task<IActionResult> GetLoggedUser()
        {
            Guid correlationId = Guid.NewGuid();
            try
            {
                ResponseMessage<LoggedUserDTO>? response = null;
                var identityUser = _contextAccessor.HttpContext?.User?.Identity;
                if (identityUser != null && identityUser.IsAuthenticated)
                {
                    response = await _userManagerRepository.GetUserDetails(identityUser.Name!);
                }
                else
                {
                    response = new ResponseMessage<LoggedUserDTO>();
                }

                return ResponseMessageHelper.GetResult(response);
            }
            catch (Exception exp)
            {
                _logger.LogError(exp, "Exception in GetLoggedUser", "AccountController", "GetLoggedUser", correlationId);
                return ResponseMessageHelper.ServerError(correlationId);
            }
        }
    }
}
