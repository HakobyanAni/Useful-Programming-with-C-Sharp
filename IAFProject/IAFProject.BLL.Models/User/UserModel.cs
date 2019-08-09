using System;
using System.Collections.Generic;
using System.Text;
using IAFProject.BLL.Models.General;

namespace IAFProject.BLL.Models.User
{
    public class UserModel : ModelBase
    {
        private int? _id;
        private string _name;
        private string _email;
        private string _password;
        private string _confirmPassword;
        private string _phoneNumber;

        public int? Id
        {
            get
            {
                return _id;
            }
            set
            {
                if (_id != value)
                {
                    _id = value;
                    Notify();
                }
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    Notify();
                }
            }
        }

        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                if (_email != value)
                {
                    _email = value;
                    Notify();
                }
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                if (_password != value)
                {
                    _password = value;
                    Notify();
                }
            }
        }

        public string ConfirmPassword
        {
            get { return _confirmPassword; }
            set
            {
                if (_confirmPassword != value)
                {
                    _confirmPassword = value;
                    Notify();
                }
            }
        }

        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set
            {
                if (_phoneNumber != value)
                {
                    _phoneNumber = value;
                    Notify();
                }
            }
        }

        public bool IsEmpty()
        {
            return string.IsNullOrWhiteSpace(Name)
                && string.IsNullOrWhiteSpace(Email)
                && string.IsNullOrWhiteSpace(Password)
                && string.IsNullOrWhiteSpace(ConfirmPassword)
                && string.IsNullOrWhiteSpace(PhoneNumber);
        }
    }
}
