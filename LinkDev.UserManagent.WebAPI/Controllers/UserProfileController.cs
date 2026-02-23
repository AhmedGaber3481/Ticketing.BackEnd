//using Identity.Helpers;
//using Identity.Models;
//using IdentityCore.Models;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;

//namespace Identity.Controllers
//{
//    public class UserProfileController : Controller
//    {
//        private readonly UserManager<IdentityUser> _userManager;
//        private readonly IHttpContextAccessor _contextAccessor;

//        public UserProfileController(IHttpContextAccessor contextAccessor, UserManager<IdentityUser> userManager)
//        {
//            _contextAccessor = contextAccessor;
//            _userManager = userManager;
//        }


//        [Route("Profile")]
//        public async Task<IActionResult> Index()
//        {
//            try
//            {
//                var currentUser = _contextAccessor.HttpContext?.User;

//                if (currentUser != null && currentUser.Identity != null && currentUser.Identity.IsAuthenticated)
//                {
//                    var userName = currentUser.Identity.Name;
//                    if (userName != null)
//                    {
//                        var user = await _userManager.FindByNameAsync(userName);

//                        UserProfile userProfile = new UserProfile() { UserName = user.UserName, Email = user.Email };
//                        return View("~/Pages/Profile.cshtml", userProfile);
//                    }
//                }

//                return Redirect("/Account/Login");
//            }
//            catch (Exception exp)
//            {
//                Logger.LogException(exp, "RegistrationController", "Register");

//                return View("~/Pages/ErrorPage.cshtml");
//            }
//        }

//        [Route("Account/PasswordChange")]
//        public IActionResult PasswordChange()
//        {
//            var currentUser = _contextAccessor.HttpContext?.User;
//            bool isAuthenticated = currentUser?.Identity?.IsAuthenticated ?? false;
//            if (isAuthenticated)
//            {
//                var changePassword = new ChangePassword()
//                {
//                    UserName = currentUser?.Identity?.Name
//                };

//                return View("~/Pages/ChangePassword.cshtml", changePassword);
//            }

//            return View("~/Pages/ChangePassword.cshtml");
//        }


//        [Route("ChangePassword"), HttpPost]
//        public async Task<IActionResult> ChangePassword([FromBody] ChangePassword changePassword)
//        {
//            ResponseMessage<bool> response = new ResponseMessage<bool>();
//            try
//            {
//                if (ModelState.IsValid)
//                {
//                    var user = await _userManager.FindByNameAsync(changePassword.UserName);

//                    if (user != null)
//                    {
//                        var result = await _userManager.ChangePasswordAsync(user, changePassword.Password, changePassword.NewPassword);
//                        if (result != null)
//                        {
//                            if (result.Succeeded)
//                            {
//                                response.Data = true;
//                            }
//                            else if (result.Errors != null)
//                            {
//                                response.Notifications = result.Errors.Select(x => x.Description);
//                            }
//                        }
//                    }
//                    else
//                    {
//                        response.Notifications = new string[] { "Invalid user name or password." };
//                    }
//                }
//                else
//                {
//                    response.Notifications = ErrorMessageHelper.GetErrorMessages(ModelState);
//                }

//                return ResponseMessageHelper.Ok(response);
//            }
//            catch (Exception exp)
//            {
//                Logger.LogException(exp, "RegistrationController", "Register");
//                return ResponseMessageHelper.ServerError(response);
//            }
//        }
//    }
//}
