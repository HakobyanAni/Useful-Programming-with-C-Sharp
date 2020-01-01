using System.Threading.Tasks;
using IAFProject.BLL.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using IAFProject.Authentication.Implementation;

namespace IAFProject.WebApi.Controllers
{
    public class UserController : BaseIAFController
    {
        #region Fields
        private AccountService _account;
        private AppSettings _appSettings;
        #endregion

        #region Constructors
        public UserController(AccountService account, IOptions<AppSettings> appSettings)
        {
            _account = account;
            _appSettings = appSettings.Value;
        }
        #endregion

        #region Actions
        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> SignUp(UserModel userModel)
        {
            string result = await _account.SignUp(userModel);
            return Ok(result);
        }

        [HttpGet, AllowAnonymous]
        public async Task<IActionResult> EmailConfirm(UserModel user)
        {
            string result = await _account.CheckConfirmationCodeAndRegisterUser(user);
            return Ok(result);
        }

        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginModel loginModel)
        {
            string result = await _account.Login(loginModel, _appSettings.Secret);
            return Ok(result);
        }

        [HttpGet, AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            string result = await _account.ForgotPassword(email);
            return Ok(result);
        }

        [HttpPost, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel resetPasswordModel)
        {
            string result = await _account.ResetPassword(resetPasswordModel);
            return Ok(result);
        }

        [HttpPost, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> ChangeEmail(ChangeEmailModel changeEmailModel)
        {
            string result = await _account.ChangeEmail(changeEmailModel);
            return Ok(result);
        }

        [HttpPost, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel changePasswordModel)
        {
            string result = await _account.ChangePassword(changePasswordModel);
            return Ok(result);
        }

        [HttpGet, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> LogOut()
        {
            string result = await _account.LogOut();
            return Ok(result);
        }

        [HttpPost, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteUser(string userName)
        {
            string result = await _account.Delete(userName);
            return Ok(result);
        }
        #endregion
    }
}
