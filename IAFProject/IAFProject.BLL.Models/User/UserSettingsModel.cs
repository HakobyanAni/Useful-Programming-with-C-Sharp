using IAFProject.BLL.Models.General;

namespace IAFProject.BLL.Models.User
{
    public class UserSettingsModel : ModelBase
    {
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}