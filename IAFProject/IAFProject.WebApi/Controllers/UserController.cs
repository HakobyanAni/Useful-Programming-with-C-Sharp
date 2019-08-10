using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IAFProject.BLL.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IAFProject.Authentication;

namespace IAFProject.WebApi.Controllers
{
    public class UserController : BaseIAFController
    {
        private Account _account;
        public UserController(Account account)
        {
            _account = account;
        }

        //[HttpPost]
        //[AllowAnonymous]
        //public async Task<IActionResult> SignUp(UserModel userModel)
        //{
        //    _account.SignUp(userModel);







        //    if (identityResult.Errors.Any())
        //    {
        //        return CreateDefaultResponse(identityResult.Errors.FirstOrDefault().Description);
        //    }

        //    return CreateDefaultResponse(user);
        //    return Ok();

        //}
    }
}
