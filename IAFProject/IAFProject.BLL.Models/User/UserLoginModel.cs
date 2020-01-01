using IAFProject.BLL.Models.General;

namespace IAFProject.BLL.Models.User
{
    public class UserLoginModel : ModelBase
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}