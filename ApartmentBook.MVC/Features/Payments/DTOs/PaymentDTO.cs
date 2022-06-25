using ApartmentBook.MVC.Features.Payments.Models;

namespace ApartmentBook.MVC.Features.Payments.DTOs
{
    public class PaymentDTO
    {
        public Guid Id { get; set; }
        public PaymentType Type { get; set; }
        public decimal Amount { get; set; }
        public decimal AmountPaid { get; set; }

        public bool IsPaid
        { get { return AmountPaid >= Amount; } }

        public Month PaymentMonth { get; set; }
        public int PaymentYear { get; set; }
    }
}