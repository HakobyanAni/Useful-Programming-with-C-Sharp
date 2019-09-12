﻿using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IAFProject.BLL.Models.User;
using Microsoft.AspNetCore.Identity;
using IAFProject.DAL.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using IAFProject.BLL.Models.General;

namespace IAFProject.Authentication
{
    public class AccountService : IAccountService
    {
        #region Fields
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private EmailSenderService _senderService;
        #endregion

        #region Constructors
        public AccountService(UserManager<User> userManager, SignInManager<User> signInManager, EmailSenderService senderService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _senderService = senderService;
        }
        #endregion

        #region Methods
        public async Task<object> SignUp(UserModel userModel)
        {
            User user = await _userManager.FindByEmailAsync(userModel.Email);
            if (user != null && !user.Deleted)
            {
                if (!user.EmailConfirmed)
                {
                    throw new Exception("Please, confirm your email.");
                }
                throw new Exception("User with current email already exists");
            }

            if (!string.Equals(userModel.Password, userModel.ConfirmPassword))
            {
                throw new Exception("Passwords mismatch.");
            }

            user = new User
            {
                Name = userModel.Name,
                UserName = userModel.Email,
                Email = userModel.Email,
                EmailConfirmed = false,
                PhoneNumber = userModel.PhoneNumber,
                CreateDate = DateTime.Now,
                Deleted = false
            };

            IdentityResult identityResult = await _userManager.CreateAsync(user, userModel.Password);
            if (identityResult.Errors.Any())
            {
                return identityResult.Errors.FirstOrDefault().Description;
            }

            string messageSubject = Constants.messageSubjectForUserSignUp;
            string messageBody = Constants.messageBodyForUserSignUp + $"{user.UserName}";
            string sendingMessage = await _senderService.SendEmail(user.UserName, messageSubject, messageBody);
            return user;
        }

        public async Task<string> Login(UserLoginModel loginModel, string appSecret)
        {
            User user = await _userManager.FindByNameAsync(loginModel.Username);
            if (user == null || user.Deleted)
            {
                throw new Exception("User not found!");
            }

            var passwordCheck = await _signInManager.CheckPasswordSignInAsync(user, loginModel.Password, true);
            if (!passwordCheck.Succeeded)
            {
                throw new Exception("Wrong password!");
            }

            if (!user.EmailConfirmed)
            {
                throw new Exception("Email isn't confirmed.");
            }

            user.LastEntryDate = DateTime.Now;

            SecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appSecret);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName.ToString()),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            string tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }

        public async Task<object> Update(UserUpdateModel updateModel)
        {
            User user = await _userManager.FindByNameAsync(updateModel.Name);
            if (user.Deleted || !user.EmailConfirmed)
            {
                return "User doesn't exist to update.";
            }

            user.Name = updateModel.NewName;
            user.PhoneNumber = updateModel.NewPhoneNumber;
            user.ModifiedDate = DateTime.Now;
            IdentityResult result = await _userManager.UpdateAsync(user);
            if (result.Errors.Any())
            {
                return result.Errors.FirstOrDefault().Description;
            }
            return user;
        }

        public async Task<string> EmailConfirmBL(string userName)
        {
            User user = await _userManager.FindByNameAsync(userName);
            if (user == null || user.Deleted || !user.IsRegistered)
            {
                throw new Exception("Cannot find user.");
            }
            user.EmailConfirmed = true;

            IdentityResult result = await _userManager.UpdateAsync(user);
            if (result.Errors.Any())
            {
                return result.Errors.FirstOrDefault().Description;
            }
            return "Email successfully confirmed. Now you can log in to your account.";
        }

        public async Task<string> ChangePasswordBL(ChangePasswordModel changePasswordModel)
        {
            if (changePasswordModel.NewPassword != changePasswordModel.ConfirmPassword)
            {
                throw new Exception("Passwords mismatch.");
            }

            User user = await _userManager.FindByNameAsync(changePasswordModel.UserName);
            if (user == null || user.Deleted || !user.EmailConfirmed)
            {
                throw new Exception("User doesn't exist.");
            }

            IdentityResult result = await _userManager.ChangePasswordAsync(user, changePasswordModel.CurrentPassword, changePasswordModel.NewPassword);
            if (!result.Succeeded)
            {
                return result.Errors.FirstOrDefault().Description;
            }
            return "Password was successfully changed.";
        }

        public async Task<string> Delete(string userName)
        {
            User userToDelete = await _userManager.FindByNameAsync(userName);
            if (userToDelete == null || userToDelete.Deleted)
            {
                throw new Exception("User doesn't exist.");
            }

            userToDelete.Deleted = true;
            userToDelete.ModifiedDate = DateTime.Now;

            IdentityResult result = await _userManager.UpdateAsync(userToDelete);
            if (result.Errors.Any())
            {
                return result.Errors.FirstOrDefault().Description;
            }
            return "User was deleted.";
        }
        #endregion
    }
}