using System.Threading.Tasks;

namespace CampusActivityHub.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string message);
    }

    public class EmailService : IEmailService
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            System.Console.WriteLine($"[EMAIL MOCK] To: {email}, Subject: {subject}");
            return Task.CompletedTask;
        }
    }
}