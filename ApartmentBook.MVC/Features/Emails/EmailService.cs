using FluentEmail.Core;
using Microsoft.AspNetCore.Identity.UI.Services;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace ApartmentBook.MVC.Features.Emails
{
    public class EmailService : IEmailSender, IEmailService
    {
        private readonly ILogger<EmailService> logger;
        private readonly IConfiguration configuration;
        private readonly IFluentEmail fluentEmail;

        public EmailService(ILogger<EmailService> logger, IConfiguration configuration, IFluentEmail fluentEmail)
        {
            this.logger = logger;
            this.configuration = configuration;
            this.fluentEmail = fluentEmail;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            var sendGridKey = configuration["SendGrid:Key"];
            if (string.IsNullOrEmpty(sendGridKey))
            {
                throw new Exception("SendGridKey not found!");
            }
            var result = await Execute(sendGridKey, subject, message, toEmail);
        }

        private async Task<bool> Execute(string apiKey, string subject, string message, string toEmail, string title = "Apartment Book")
        {
            var from = configuration.GetSection("SendGrid:Email").Value;

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
            if (!response.IsSuccessStatusCode)
            {
                logger.LogError("Failed to send email");
                return false;
            }
            logger.LogInformation("Email queued successfully");
            return true;
        }
    }
}