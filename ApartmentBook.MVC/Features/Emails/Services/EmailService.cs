using ApartmentBook.MVC.Features.Emails.Models;
using ApartmentBook.MVC.Features.Payments.Models;
using ApartmentBook.MVC.Features.Payments.Services;
using ApartmentBook.MVC.Features.Tenants.Services;
using FluentEmail.Core;
using FluentEmail.Core.Models;
using FluentEmail.SendGrid;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace ApartmentBook.MVC.Features.Emails.Services
{
    public class EmailService : IEmailSender, IEmailService
    {
        private readonly ILogger<EmailService> logger;
        private readonly IConfiguration configuration;
        private readonly IPaymentService paymentService;
        private readonly ITenantService tenantService;

        public EmailService(ILogger<EmailService> logger, IConfiguration configuration,
            IPaymentService paymentService, ITenantService tenantService)
        {
            this.logger = logger;
            this.configuration = configuration;
            this.paymentService = paymentService;
            this.tenantService = tenantService;
        }

        public async Task SendEmailAsync(string to, string subject, string message)
        {
            var sendGridKey = configuration["SendGrid:Key"];
            if (string.IsNullOrEmpty(sendGridKey))
            {
                logger.LogInformation("SendGrid key not found!");
                throw new Exception("SendGridKey not found!");
            }
            var result = await Execute(sendGridKey, to, message, subject);
        }

        private async Task<bool> Execute(string apiKey, string to, string message, string subject = "Apartment Book")
        {
            var from = configuration.GetSection("SendGrid:Email").Value;
            //var sendGridClient = new SendGridClient(apiKey);
            var messageTemplatePath = Path.Combine(Environment.CurrentDirectory, @"Features\Emails\Templates\PaymentReminderEmailTemplate.cshtml");

            IFluentEmail fluentEmail = Email
                .From(from)
                .To(to)
                .Subject(subject)
                .Tag("ApartmentBook")
                .UsingTemplateFromFile(messageTemplatePath, new PaymentReminderEmail
                {
                    Name = "<Your Name>",
                    Payments = new List<Payment>() {
                new Payment()
                {
                    Amount = 2
                }
                }
                }); ;

            var sendGridSender = new SendGridSender(apiKey);
            SendResponse response = sendGridSender.Send(fluentEmail);

            if (!response.Successful)
            {
                logger.LogError("Failed to send email! Check the errors: ");
                foreach (string error in response.ErrorMessages)
                {
                    Console.WriteLine(error);
                }
            }

            logger.LogInformation("Email send successfully");
            return true;
        }
    }
}