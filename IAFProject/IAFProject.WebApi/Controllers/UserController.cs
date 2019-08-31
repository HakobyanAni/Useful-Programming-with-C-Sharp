using System.Threading.Tasks;
using IAFProject.BLL.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IAFProject.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using IAFProject.DAL.Models;

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
        [HttpPost]
        public async Task<IActionResult> SignUp(UserModel userModel)
        {
            var result = await _account.SignUp(userModel);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> EmailConfirm(string userName)
        {
            string result = await _account.EmailConfirmBL(userName);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginModel loginModel)
        {
            string result = await _account.Login(loginModel, _appSettings.Secret);
            return Ok(result);
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Update(UserUpdateModel userModel)
        {
            var result = await _account.Update(userModel);
            return Ok(result);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel changePasswordModel)
        {
            string result = await _account.ChangePasswordBL(changePasswordModel);
            return Ok(result);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteUser(string userName)
        {
            string result = await _account.Delete(userName);
            return Ok(result);
        }
        #endregion
    }
}
