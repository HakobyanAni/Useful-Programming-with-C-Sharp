using System;
using System.Collections.Generic;
using System.Text;

namespace IAFProject.BLL.Models.User
{
    public class UserUpdateModel
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public string NewName { get; set; }
        public string PhoneNumber { get; set; }
        public string NewPhoneNumber { get; set; }
    }
}
