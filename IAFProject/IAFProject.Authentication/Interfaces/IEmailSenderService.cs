using System.Threading.Tasks;

namespace IAFProject.Authentication.Interfaces
{
    public interface IEmailSenderService
    {
        Task<string> SendEmail(string email, string subject, string body);
    }
}