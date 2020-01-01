using IAFProject.BLL.Models.General;

namespace IAFProject.BLL.Models.User
{
    public class ResetPasswordModel : ModelBase
    {
        public string UserName { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public string ConfirmationCode { get; set; }
    }
}
