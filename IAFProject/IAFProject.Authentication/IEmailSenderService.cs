using System.Threading.Tasks;

namespace IAFProject.Authentication
{
    public interface IEmailSenderService
    {
        Task<string> SendEmail(string userName, string messageSubject, string messageBody);
    }
}