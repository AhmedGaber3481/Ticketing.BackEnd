using LinkDev.Ticketing.API.Helpers;
using LinkDev.UserManagent.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LinkDev.Ticketing.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AboutController : ControllerBase
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly LinkDev.Ticketing.Logging.Application.Interfaces.ILogger _logger;
        
        public AboutController(IHttpContextAccessor contextAccessor
            , UserManager<IdentityUser> userManager
            , LinkDev.Ticketing.Logging.Application.Interfaces.ILogger logger
            , RoleManager<IdentityRole> roleManager)
        {
            _contextAccessor = contextAccessor;
            _userManager = userManager;
            _logger = logger;
            
        }

        [HttpGet]
        public ActionResult About()
        {
            return Ok(new { Version = 1 });
        }


        [Route("GetUserProfile"), HttpGet]
        public async Task<IActionResult> GetUserProfile()
        {
            Guid correlationId = Guid.NewGuid();
            try
            {
                var currentUser = _contextAccessor.HttpContext?.User;

                if (currentUser?.IsInRole("Admin") ?? false)
                {

                }
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

    }
}
