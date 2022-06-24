namespace ApartmentBook.MVC.Features.Emails
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string message);
    }
}