using LinkDev.Ticketing.Core.Helpers;
using LinkDev.UserManagent.Domain.Models;
using LinkDev.UserManagent.WebAPI.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Controllers
{
    [Route("api/UserProfile")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly LinkDev.Ticketing.Logging.Application.Interfaces.ILogger _logger;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserProfileController(IHttpContextAccessor contextAccessor
            , UserManager<IdentityUser> userManager
            , LinkDev.Ticketing.Logging.Application.Interfaces.ILogger logger
            , RoleManager<IdentityRole> roleManager)
        {
            _contextAccessor = contextAccessor;
            _userManager = userManager;
            _logger = logger;
            _roleManager = roleManager;
        }


        [Route("GetUserProfile"), HttpGet]
        public async Task<IActionResult> GetUserProfile()
        {
            Guid correlationId = Guid.NewGuid();
            try
            {
                var currentUser = _contextAccessor.HttpContext?.User;

                if (currentUser != null && currentUser.Identity != null && currentUser.Identity.IsAuthenticated)
                {
                    var userName = currentUser.Identity.Name;
                    if (userName != null)
                    {
                        var user = await _userManager.FindByNameAsync(userName);

                        if (user != null)
                        {
                            UserProfile userProfile = new UserProfile() { UserName = user.UserName, Email = user.Email };

                            return ResponseMessageHelper.Ok(userProfile);
                        }
                        return ResponseMessageHelper.Ok(new UserProfile());
                    }
                }

                return Redirect("/Account/Login");
            }
            catch (Exception exp)
            {
                _logger.LogError(exp, "Exception in GetUserProfile", "UserProfileController", "GetUserProfile", correlationId);

                return ResponseMessageHelper.ServerError(correlationId);
            }
        }

        //[HttpGet]
        //[Route("SeedRoles")]
        //public async Task<IActionResult> SeedRoles()
        //{
        //    string[] roles = { "Admin", "HelpDesk", "Client" };

        //    foreach (var role in roles)
        //    {
        //        if (!await _roleManager.RoleExistsAsync(role))
        //        {
        //            await _roleManager.CreateAsync(new IdentityRole(role));
        //        }
        //    }
            
        //    return Ok(new { roles = roles });
        //}

        //[Route("Account/PasswordChange")]
        //public IActionResult PasswordChange()
        //{
        //    var currentUser = _contextAccessor.HttpContext?.User;
        //    bool isAuthenticated = currentUser?.Identity?.IsAuthenticated ?? false;
        //    if (isAuthenticated)
        //    {
        //        var changePassword = new ChangePassword()
        //        {
        //            UserName = currentUser?.Identity?.Name
        //        };

        //        return View("~/Pages/ChangePassword.cshtml", changePassword);
        //    }

        //    return View("~/Pages/ChangePassword.cshtml");
        //}


        //[Route("ChangePassword"), HttpPost]
        //public async Task<IActionResult> ChangePassword([FromBody] ChangePassword changePassword)
        //{
        //    ResponseMessage<bool> response = new ResponseMessage<bool>();
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            var user = await _userManager.FindByNameAsync(changePassword.UserName);

        //            if (user != null)
        //            {
        //                var result = await _userManager.ChangePasswordAsync(user, changePassword.Password, changePassword.NewPassword);
        //                if (result != null)
        //                {
        //                    if (result.Succeeded)
        //                    {
        //                        response.Data = true;
        //                    }
        //                    else if (result.Errors != null)
        //                    {
        //                        response.Notifications = result.Errors.Select(x => x.Description);
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                response.Notifications = new string[] { "Invalid user name or password." };
        //            }
        //        }
        //        else
        //        {
        //            response.Notifications = ErrorMessageHelper.GetErrorMessages(ModelState);
        //        }

        //        return ResponseMessageHelper.Ok(response);
        //    }
        //    catch (Exception exp)
        //    {
        //        Logger.LogException(exp, "RegistrationController", "Register");
        //        return ResponseMessageHelper.ServerError(response);
        //    }
        //}
    }
}
