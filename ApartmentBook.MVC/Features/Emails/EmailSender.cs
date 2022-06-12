using Microsoft.AspNetCore.Identity.UI.Services;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace ApartmentBook.MVC.Features.Emails
{
    public class EmailSender : IEmailSender
    {
        private readonly ILogger logger;
        private readonly IConfiguration configuration;

        public EmailSender(ILogger<EmailSender> logger, IConfiguration configuration)
        {
            this.logger = logger;
            this.configuration = configuration;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            var sendGridKey = configuration["SendGrid:Key"];
            if (string.IsNullOrEmpty(sendGridKey))
            {
                throw new Exception("SendGridKey not found!");
            }
            await Execute(sendGridKey, subject, message, toEmail);
        }

        public async Task Execute(string apiKey, string subject, string message, string toEmail, string title = "Apartment Book")
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(configuration["SendGrid:Email"], title),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(toEmail));

            var response = await client.SendEmailAsync(msg);
            if (response.IsSuccessStatusCode)
            {
                logger.LogInformation("Email queued successfully");
            }
            else
            {
                logger.LogError("Failed to send email");
            }
        }
    }
}