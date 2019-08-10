using System.Threading.Tasks;
using IAFProject.BLL.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IAFProject.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace IAFProject.WebApi.Controllers
{
    public class UserController : BaseIAFController
    {
        private Account _account;
        private AppSettings _appSettings;

        public UserController(Account account, IOptions<AppSettings> appSettings)
        {
            _account = account;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SignUp(UserModel userModel)
        {
            var result = await _account.SignUp(userModel);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginModel loginModel)
        {
            var result = await _account.Login(loginModel, _appSettings.Secret);
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
        public async Task<IActionResult> DeleteUser(string userName)
        {
            string result = await _account.Delete(userName);
            return Ok(result);
        }
    }
}
