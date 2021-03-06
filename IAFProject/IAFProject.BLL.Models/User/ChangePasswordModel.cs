﻿using IAFProject.BLL.Models.General;

namespace IAFProject.BLL.Models.User
{
    public class ChangePasswordModel : ModelBase
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
