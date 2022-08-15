namespace ApartmentBook.MVC.Features.Emails.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string message);
    }
}