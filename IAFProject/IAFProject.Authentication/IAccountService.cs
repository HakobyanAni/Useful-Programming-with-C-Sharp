using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using IAFProject.BLL.Models.User;
using Microsoft.AspNetCore.Identity;

namespace IAFProject.Authentication
{
    public interface IAccountService
    {
        Task<object> SignUp(UserModel userModel);
        Task<string> Login(UserLoginModel loginModel, string appSecret);
        Task<object> Update(UserUpdateModel updateModel);
        Task<string> Delete(string userName);
    }
}
