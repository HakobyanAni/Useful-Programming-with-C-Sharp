using System;
using System.Threading.Tasks;
using IAFProject.Authentication;
using IAFProject.BLL.Interfaces;
using IAFProject.DAL.Models;
using Microsoft.AspNetCore.Identity;
using IAFProject.BLL.Models.General;
using IAFProject.Authentication.Implementation;

namespace IAFProject.BLL.UserDeleteProcessing
{
    public class UserDeleteProcessor : IUserDeleteProcessor
    {
        #region Fields
        private readonly UserManager<User> _userManager;
        private readonly EmailSenderService _emailSenderService;
        #endregion

        #region Constructors
        public UserDeleteProcessor(UserManager<User> userManager, EmailSenderService emailSenderService)
        {
            _userManager = userManager;
            _emailSenderService = emailSenderService;
        }
        #endregion

        #region Methods
        public async Task StartProcessingAsync()
        {
            var allUsers = await _userManager.GetUsersInRoleAsync("User");
            foreach (var user in allUsers)
            {
                if (!user.Deleted)
                {
                    int now = DateTime.Now.Year;
                    int lastEntry = user.LastEntryDate.HasValue ? user.LastEntryDate.Value.Year : 0;
                    if (now - lastEntry > 1)
                    {
                        string messageSubject = Constants.messageSubjectForUserDeleteJob;
                        string messageBody = Constants.messageBodyForUserDeleteJob;
                        var sendEmail = await _emailSenderService.SendEmail(user.UserName, messageSubject, messageBody);
                        IdentityResult result = await _userManager.DeleteAsync(user);
                    }
                }
            }
        }
        #endregion
    }
}