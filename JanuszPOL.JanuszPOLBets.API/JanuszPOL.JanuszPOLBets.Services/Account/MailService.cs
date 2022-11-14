using Microsoft.Extensions.Options;
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
        private readonly IOptions<ResetOptions> _resetOption;

        public MailService(IOptions<ResetOptions> resetOption)
        {
            _resetOption = resetOption;
        }

            public async Task SendEmailAsync(string toEmail, string subject, string content)
            {
                var apiKey = _resetOption.Value.SendGridAPIKey;
                var client = new SendGridClient(apiKey);
                var from = new EmailAddress("januszpol.eu@gmail.com", "JanuszPol Bet");
                var to = new EmailAddress(toEmail);
                var msg = MailHelper.CreateSingleEmail(from, to, subject, content, content);
                var response = await client.SendEmailAsync(msg);
            }
    }
}
