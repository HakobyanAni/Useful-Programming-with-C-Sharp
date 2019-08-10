using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IAFProject.BLL.Models.User;
using Microsoft.AspNetCore.Identity;
using IAFProject.BLL.Models.General;
using IAFProject.DAL.Models;

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

        public async Task SignUp(UserModel userModel)
        {
        }

        public async Task Login(UserLoginModel loginModel)
        {

        }

        public async Task Update(UserUpdateModel updateModel)
        {

        }

        public async Task Delete(string userName)
        {

        }

    }
}