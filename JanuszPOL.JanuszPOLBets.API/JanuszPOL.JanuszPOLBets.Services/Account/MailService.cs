using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace JanuszPOL.JanuszPOLBets.Services.Account
{
    public interface IMailService
    {
        Task SendEmailAsync(string toEmail, string subject, string content);
    }
    public class MailService : IMailService
    {
        
            private IConfiguration _configuration;

            public MailService(IConfiguration configuration)
            {
                _configuration = configuration;
            }

            public async Task SendEmailAsync(string toEmail, string subject, string content)
            {
                var apiKey = _configuration["SendGridAPIKey"];
                var client = new SendGridClient(apiKey);
                var from = new EmailAddress("januszpol.eu@gmail.com", "JanuszPol Bet");
                var to = new EmailAddress(toEmail);
                var msg = MailHelper.CreateSingleEmail(from, to, subject, content, content);
                var response = await client.SendEmailAsync(msg);
            }
    }
}
