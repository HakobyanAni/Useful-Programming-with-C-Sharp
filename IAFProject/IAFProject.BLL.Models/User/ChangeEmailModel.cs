using IAFProject.BLL.Models.General;

namespace IAFProject.BLL.Models.User
{
    public class ChangeEmailModel : ModelBase
    {
        public string CurrentEmail { get; set; }
        public string NewEmail { get; set; }
    }
}