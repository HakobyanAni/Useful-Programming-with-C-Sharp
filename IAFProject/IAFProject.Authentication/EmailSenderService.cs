using System.Net.Mail;
using System.Threading.Tasks;
using IAFProject.DAL.Models;
using Microsoft.AspNetCore.Identity;

namespace IAFProject.Authentication
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
        public async Task<string> SendEmail(string userName, string messageSubject, string messageBody)
        {
            User user = await _userManager.FindByNameAsync(userName);
            MailMessage mailMessage = new MailMessage();
            SmtpClient server = new SmtpClient();

            mailMessage.From = new MailAddress("AT-Tech.10@gmail.com");
            mailMessage.To.Add(user.Email);
            mailMessage.Subject = messageSubject;
            mailMessage.Body = messageBody;
            mailMessage.IsBodyHtml = true;

            server.Credentials = new System.Net.NetworkCredential("AT-Tech.10@gmail.com", "hi-&High08Tech");
            server.EnableSsl = true;
            server.Host = "smtp.gmail.com";
            server.Port = 587;
            server.Send(mailMessage);
            return "Email was sent";
        }
        #endregion
    }
}
