using ApartmentBook.MVC.Features.Payments.Models;

namespace ApartmentBook.MVC.Features.Emails.Models
{
    public class PaymentReminderEmail
    {
        public string Name { get; set; } = "Maciek";
        public IEnumerable<Payment> Payments { get; set; }
    }
}