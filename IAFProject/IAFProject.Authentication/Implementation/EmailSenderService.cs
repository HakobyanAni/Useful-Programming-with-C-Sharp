using IAFProject.Authentication.Interfaces;
using IAFProject.DAL.Models;
using IAFProject.General.Utils;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace IAFProject.Authentication.Implementation
{
    public class EmailSenderService : IEmailSenderService
    {
        #region Fields
        private readonly UserManager<User> _userManager;
        #endregion

        #region Constructor
        public EmailSenderService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        #endregion

        #region Methods
        public async Task<string> SendEmail(string email, string subject, string body)
        {
            if (!ValidationService.IsEmailValid(email))
            {
                throw new Exception("The email isn't valid.");
            }

            User user = await _userManager.FindByEmailAsync(email);
            if (user == null || user.Deleted)
            {
                throw new Exception("User doesn't exist.");
            }

            try
            {
                MailMessage mailMessage = new MailMessage();
                SmtpClient server = new SmtpClient();

                mailMessage.From = new MailAddress("CompanyGmailInfo@gmail.com");
                mailMessage.To.Add(email);
                mailMessage.Subject = subject;

                mailMessage.Body = body;
                mailMessage.IsBodyHtml = true;

                server.Credentials = new System.Net.NetworkCredential("CompanyGmailInfo@gmail.com", "GmailPassword");
                server.EnableSsl = true;
                server.Host = "smtp.gmail.com";
                server.Port = 587;
                server.Send(mailMessage);

                user.ConfirmationCode = body;
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }

            IdentityResult res = await _userManager.UpdateAsync(user);
            if (res.Errors.Any())
            {
                throw new Exception(res.Errors.FirstOrDefault().Description);
            }
            return "Confiramtion code was sent to your email.";
        }
        #endregion
    }
}