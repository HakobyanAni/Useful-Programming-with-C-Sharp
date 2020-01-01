using IAFProject.Authentication.Interfaces;
using IAFProject.BLL.Models.User;
using IAFProject.DAL.Models;
using IAFProject.General.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using IAFProject.BLL.Models.General;

namespace IAFProject.Authentication.Implementation
{
    public class AccountService : IAccountService
    {
        #region Fields
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private EmailSenderService _senderService;
        private IHttpContextAccessor _httpContextAccessor;
        #endregion

        #region Constructors
        public AccountService(UserManager<User> userManager, SignInManager<User> signInManager, EmailSenderService senderService, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _senderService = senderService;
            _httpContextAccessor = httpContextAccessor;
        }
        #endregion

        #region Methods
        public async Task<string> SignUp(UserModel userModel)
        {
            if (!ValidationService.IsUserModelValid(userModel))
            {
                throw new Exception("User model is not valid.");
            }
            if (!ValidationService.IsEmailValid(userModel.Email))
            {
                throw new Exception("Email isn't valid.");
            }
            if (!ValidationService.IsPasswordValid(userModel.Password))
            {
                throw new Exception("Password must be at least 8 characters, contain at least 1 digital, 1 lower case letter, 1 upper case letter, " +
                                    "1 non-character (such as !,#,%,@, etc)");
            }

            User user = await _userManager.FindByEmailAsync(userModel.Email);
            if (user != null)
            {
                if (user.IsRegistered && !user.Deleted)
                {
                    if (!user.EmailConfirmed)
                    {
                        throw new Exception("Please, confirm your email.");
                    }
                    else
                    {
                        throw new Exception("User with current email already exists.");
                    }
                }
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
                IsRegistered = false,
                Deleted = false
            };

            IdentityResult identityResult = await _userManager.CreateAsync(user, userModel.Password);
            if (identityResult.Errors.Any())
            {
                return identityResult.Errors.FirstOrDefault().Description;
            }

            string messageSubject = Constants.messageSubjectForUserSignUp;
            Random random = new Random();
            string confirmationCode = (random.Next(1000, 9999)).ToString();
            string result = await _senderService.SendEmail(userModel.Email, messageSubject, confirmationCode);
            return result;
        }

        public async Task<string> Login(UserLoginModel loginModel, string appSecret)
        {
            if (loginModel.Username == null || loginModel.Password == null)
            {
                throw new Exception("Sorry, you cannot log in. Please, fill all fields.");
            }
            if (!ValidationService.IsEmailValid(loginModel.Username))
            {
                throw new Exception("Email isn't valid.");
            }
            if (!ValidationService.IsPasswordValid(loginModel.Password))
            {
                throw new Exception("Password must be at least 8 characters, contain at least 1 digital, 1 lower case letter, 1 upper case letter, " +
                                    "1 non-character (such as !,#,%,@, etc)");
            }

            User user = await _userManager.FindByEmailAsync(loginModel.Username);
            if (user == null || user.Deleted || !user.IsRegistered)
            {
                throw new Exception("User doesn't exist.");
            }
            if (!user.EmailConfirmed)
            {
                throw new Exception("Email isn't confirmed.");
            }

            var passwordCheck = await _signInManager.CheckPasswordSignInAsync(user, loginModel.Password, true);
            if (!passwordCheck.Succeeded)
            {
                throw new Exception("Wrong password!");
            }

            user.LastEntryDate = DateTime.Now;
            IdentityResult res = await _userManager.UpdateAsync(user);

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

        public async Task<string> CheckConfirmationCodeAndRegisterUser(UserModel user)
        {
            User currentUser = await _userManager.FindByEmailAsync(user.Email);
            if (currentUser == null || currentUser.Deleted)
            {
                throw new Exception("User doesn't exist.");
            }
            if (currentUser.ConfirmationCode == user.ConfirmationCode)
            {
                currentUser.IsRegistered = true;
                currentUser.EmailConfirmed = true;
                IdentityResult result = await _userManager.UpdateAsync(currentUser);
                if (result.Errors.Any())
                {
                    throw new Exception(result.Errors.FirstOrDefault().Description);
                }
            }
            else
            {
                throw new Exception("Confirmation codes mismatch.");
            }
            return "User successfully created.";
        }

        public async Task<string> ForgotPassword(string email)
        {
            if (!ValidationService.IsEmailValid(email))
            {
                throw new Exception("Email isn't valid.");
            }
            if (email == null)
            {
                throw new Exception("Please, fill the email field.");
            }
            User user = await _userManager.FindByEmailAsync(email);
            if (user == null || user.Deleted || !user.IsRegistered)
            {
                throw new Exception("User doesn't exist.");
            }

            string messageSubject = Constants.messageSubjectForUserSignUp;
            Random random = new Random();
            string confirmationCode = (random.Next(1000, 9999)).ToString();
            string result = await _senderService.SendEmail(email, messageSubject, confirmationCode);
            return result;
        }

        public async Task<string> ResetPassword(ResetPasswordModel resetPasswordModel)
        {
            if (!ValidationService.IsPasswordValid(resetPasswordModel.NewPassword))
            {
                throw new Exception("Password must be at least 8 characters, contain at least 1 digital, 1 lower case letter, 1 upper case letter, " +
                                  "1 non-character (such as !,#,%,@, etc)");
            }
            if (resetPasswordModel.NewPassword != resetPasswordModel.ConfirmPassword)
            {
                throw new Exception("Passwords mismatch.");
            }

            User user = await _userManager.FindByEmailAsync(resetPasswordModel.UserName);
            if (user == null || user.Deleted || !user.EmailConfirmed)
            {
                throw new Exception("User doesn't exist.");
            }

            if (user.ConfirmationCode != resetPasswordModel.ConfirmationCode)
            {
                throw new Exception("Confirmation code isn't correct");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            IdentityResult result = await _userManager.ResetPasswordAsync(user, token, resetPasswordModel.NewPassword);
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.FirstOrDefault().Description);
            }
            return "Password reset completed successfuly.";
        }

        public async Task<string> ChangeEmail(ChangeEmailModel changeEmailModel)
        {
            var userClaims = _httpContextAccessor.HttpContext.User;
            var currentUser = await _userManager.GetUserAsync(userClaims);

            User user = await _userManager.FindByIdAsync(currentUser.Id.ToString());
            if (user == null || user.Deleted || !user.EmailConfirmed)
            {
                throw new Exception("User doesn't exist.");
            }
            if (user.Email != changeEmailModel.CurrentEmail)
            {
                throw new Exception("Oops! Your email isn't correct.");
            }

            User anotherUser = await _userManager.FindByEmailAsync(changeEmailModel.NewEmail);
            if (anotherUser != null)
            {
                throw new Exception("Account with your new email already exists. Use another email.");
            }

            user.Email = changeEmailModel.NewEmail;
            user.UserName = changeEmailModel.NewEmail;
            user.NormalizedEmail = changeEmailModel.NewEmail;
            user.NormalizedUserName = changeEmailModel.NewEmail;
            user.ModifiedDate = DateTime.Now;
            IdentityResult result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.FirstOrDefault().Description);
            }
            return "Email wass successfuly changed.";
        }

        public async Task<string> ChangePassword(ChangePasswordModel changePasswordModel)
        {
            if (!ValidationService.IsPasswordValid(changePasswordModel.NewPassword))
            {
                throw new Exception("Password must be at least 8 characters, contain at least 1 digital, 1 lower case letter, 1 upper case letter, " +
                                    "1 non-character (such as !,#,%,@, etc)");
            }

            var userClaims = _httpContextAccessor.HttpContext.User;
            var currentUser = await _userManager.GetUserAsync(userClaims);
            if (changePasswordModel.NewPassword != changePasswordModel.ConfirmPassword)
            {
                throw new Exception("Passwords mismatch.");
            }

            User user = await _userManager.FindByIdAsync(currentUser.Id.ToString());
            if (user == null || user.Deleted || !user.EmailConfirmed)
            {
                throw new Exception("User doesn't exist.");
            }

            bool checkPasswordWithToken = await _userManager.CheckPasswordAsync(currentUser, changePasswordModel.CurrentPassword);
            if (!checkPasswordWithToken)
            {
                throw new Exception("Your password is incorrect.");
            }

            IdentityResult result = await _userManager.ChangePasswordAsync(user, changePasswordModel.CurrentPassword, changePasswordModel.NewPassword);
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.FirstOrDefault().Description);
            }
            return "Password was successfully changed.";
        }

        public async Task<string> Delete(string userName)
        {
            User userToDelete = await _userManager.FindByNameAsync(userName);
            if (userToDelete == null || userToDelete.Deleted || !userToDelete.EmailConfirmed)
            {
                throw new Exception("User doesn't exist.");
            }
            userToDelete.Deleted = true;
            userToDelete.ModifiedDate = DateTime.Now;

            Guid g = Guid.NewGuid();
            userToDelete.UserName += g.ToString();
            userToDelete.NormalizedUserName += g.ToString();
            userToDelete.Email += g.ToString();
            userToDelete.NormalizedEmail += g.ToString();
            IdentityResult result = await _userManager.UpdateAsync(userToDelete);
            if (result.Errors.Any())
            {
                return result.Errors.FirstOrDefault().Description;
            }
            return "User was deleted.";
        }

        public async Task<string> LogOut()
        {
            try
            {
                await _signInManager.SignOutAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
            return "Logging out.";
        }
        #endregion
    }
}