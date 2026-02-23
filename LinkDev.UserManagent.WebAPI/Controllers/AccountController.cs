using LinkDev.Ticketing.Core.Models;
using LinkDev.UserManagent.Domain.Models;
using LinkDev.UserManagent.WebAPI.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly LinkDev.Ticketing.Logging.Application.Interfaces.ILogger _logger;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager,
            LinkDev.Ticketing.Logging.Application.Interfaces.ILogger logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }


        [Route("SignIn"), HttpPost]
        public async Task<IActionResult> SignIn([FromBody]User loggedUser)
        {
            ResponseMessage<bool> response = new ResponseMessage<bool>();
            Guid correlationId = Guid.NewGuid();
            try
            {
                if (ModelState.IsValid)
                {
                    IdentityUser? user = await _userManager.FindByNameAsync(loggedUser.UserName);

                    if (user != null)
                    {
                        var signInResult = await _signInManager.PasswordSignInAsync(user, loggedUser.Password, false, true);

                        if (signInResult != null)
                        {
                            if (signInResult.Succeeded)
                            {
                                response.Data = true;
                            }
                            else if (signInResult.IsLockedOut)
                            {
                                response.Data = false;
                                response.Notifications = new string[] { "User is locked" };
                            }
                            else
                            {
                                response.Notifications = new string[] { "Invalid user name or password" };
                            }
                        }
                    }
                    else
                    {
                        response.Notifications = new string[] { "Invalid user name or password" };
                    }
                }
                else
                {
                    response.Notifications = ErrorMessageHelper.GetErrorMessages(ModelState);
                }

                return ResponseMessageHelper.Ok(response);
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
    }
}
