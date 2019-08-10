using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IAFProject.BLL.Models.User;
using Microsoft.AspNetCore.Identity;
using IAFProject.DAL.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace IAFProject.Authentication
{
    public class Account
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;

        public Account(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<object> SignUp(UserModel userModel)
        {
            if (!string.Equals(userModel.Password, userModel.ConfirmPassword))
            {
                throw new Exception("Passwords mismatch.");
            }

            var user = await _userManager.FindByEmailAsync(userModel.Email);
            if (user != null)
            {
                throw new Exception("User with current email already exists");
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

            var identityResult = await _userManager.CreateAsync(user, userModel.Password);
            if (identityResult.Errors.Any())
            {
                return identityResult.Errors.FirstOrDefault().Description;
            }
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
                throw new Exception("Wrong password !");
            }

            if (!user.EmailConfirmed)
            {
                throw new Exception("Email isn't confirmed.");
            }

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
            await _userManager.UpdateAsync(user);
            return user;
        }

        public async Task<string> Delete(string userName)
        {
            User usertoDelete = await _userManager.FindByNameAsync(userName);
            if (usertoDelete == null || usertoDelete.Deleted)
            {
                throw new Exception("User doesn't exist.");
            }

            usertoDelete.Deleted = true;
            usertoDelete.ModifiedDate = DateTime.Now;
            IdentityResult result = await _userManager.UpdateAsync(usertoDelete);

            if (result.Errors.Any())
            {
                return result.Errors.FirstOrDefault().Description;
            }
            return "User was deleted.";
        }
    }
}