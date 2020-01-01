using System.Threading.Tasks;
using IAFProject.BLL.Models.User;

namespace IAFProject.Authentication.Interfaces
{
    public interface IAccountService
    {
        Task<string> SignUp(UserModel userModel);
        Task<string> Login(UserLoginModel loginModel, string appSecret);
        Task<string> CheckConfirmationCodeAndRegisterUser(UserModel user);
        Task<string> ForgotPassword(string email);
        Task<string> ResetPassword(ResetPasswordModel resetPasswordModel);
        Task<string> ChangeEmail(ChangeEmailModel changeEmailModel);
        Task<string> ChangePassword(ChangePasswordModel changePasswordModel);
        Task<string> LogOut();
        Task<string> Delete(string userName);
    }
}