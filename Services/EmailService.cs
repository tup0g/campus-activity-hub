using System.IO;
using System.Threading.Tasks;

namespace CampusActivityHub.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body);
    }

    public class FileEmailService : IEmailService
    {
        private readonly IWebHostEnvironment _env;

        public FileEmailService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            string logPath = Path.Combine(_env.ContentRootPath, "sent_emails.txt");
            string emailContent = $"--- EMAIL ---\nDATE: {DateTime.Now}\nTO: {to}\nSUBJECT: {subject}\nBODY: {body}\n----------------\n\n";
            
            await File.AppendAllTextAsync(logPath, emailContent);
        }
    }
}