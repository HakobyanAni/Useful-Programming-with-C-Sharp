using System;
using System.Collections.Generic;
using System.Text;

namespace IAFProject.BLL.Models.User
{
    public class ChangePasswordModel
    {
        public string UserName { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
