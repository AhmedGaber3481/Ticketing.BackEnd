using LinkDev.Ticketing.Core.Models;
using LinkDev.UserManagent.Domain.Models;
using LinkDev.UserManagent.WebAPI.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Controllers
{
    [Route("api/registration")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly LinkDev.Ticketing.Logging.Application.Interfaces.ILogger _logger;

        public RegistrationController(UserManager<IdentityUser> userManager, LinkDev.Ticketing.Logging.Application.Interfaces.ILogger logger)
        {
            _userManager = userManager;
            _logger = logger;
        }


        [Route("Register"), HttpPost]
        public async Task<IActionResult> Register([FromBody]RegisteredUser registeredUser)
        {
            Guid correlationId = Guid.NewGuid();
            ResponseMessage<bool> response = new ResponseMessage<bool>();
            try
            {
                if (ModelState.IsValid)
                {
                    IdentityUser user = new IdentityUser()
                    {
                        UserName = registeredUser.UserName,
                        Email = registeredUser.Email,
                        EmailConfirmed = true,
                        TwoFactorEnabled = false
                    };

                    IdentityResult? result = await _userManager.CreateAsync(user, registeredUser.Password);
                    if (result != null)
                    {
                        if (result.Succeeded)
                        {
                            response.Data = true;
                        }
                        else if(result.Errors != null)
                        {
                            response.Notifications = result.Errors.Select(x => x.Description);
                        }
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
                _logger.LogError(exp, "Exception in register user", "RegistrationController", "Register", correlationId);
                return ResponseMessageHelper.ServerError(correlationId);
            }
        }
    }
}
