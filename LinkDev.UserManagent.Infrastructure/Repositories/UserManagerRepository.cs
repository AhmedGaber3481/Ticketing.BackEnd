using Azure;
using LinkDev.UserManagent.Application.Interfaces;
using LinkDev.UserManagent.Domain.DTOs;
using LinkDev.UserManagent.Domain.Models;
using LinkDev.UserManagent.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace LinkDev.UserManagent.Infrastructure.Repositories
{
    public class UserManagerRepository : IUserManagerRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ApplicationDbContext _applicationDbContext;

        public UserManagerRepository(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ApplicationDbContext applicationDbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _applicationDbContext = applicationDbContext;
        }

        public async Task<ResponseMessage<LoggedUserDTO>> GetUserDetails(string userName)
        {
            ResponseMessage<LoggedUserDTO> response = new ResponseMessage<LoggedUserDTO>();
            IdentityUser? user = await _userManager.FindByNameAsync(userName);
            if (user != null)
            {
                var userDetails = _applicationDbContext.UserDetails.Where(x => x.UserId == user.Id).FirstOrDefault();

                response.Data = new LoggedUserDTO() { UserFullName = userDetails?.FullName, UserId = user.Id };
            }
            return response;
        }

        public async Task<ResponseMessage<LoginResultDTO>> SignIn(User loggedUser)
        {
            ResponseMessage<LoginResultDTO> response = new ResponseMessage<LoginResultDTO>();
            IdentityUser? user = await _userManager.FindByNameAsync(loggedUser.UserName);

            if (user != null)
            {
                var signInResult = await _signInManager.PasswordSignInAsync(user, loggedUser.Password, false, true);

                if (signInResult != null)
                {
                    if (signInResult.Succeeded)
                    {
                        var userDetails = _applicationDbContext.UserDetails.Where(x => x.UserId == user.Id).FirstOrDefault();

                        response.Data = new LoginResultDTO() { LoginSuccess = true, UserFullName = user.UserName, UserId = userDetails?.UserId };
                    }
                    else if (signInResult.IsLockedOut)
                    {
                        response.Data = new LoginResultDTO() { LoginSuccess = false };
                        response.Notifications = new string[] { "User is locked" };
                    }
                    else
                    {
                        response.Notifications = new string[] { "Invalid user name or password" };
                    }
                }
            }
            return response;
        }
    }
}
