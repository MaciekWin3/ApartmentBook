using ApartmentBook.MVC.Features.Payments.Models;

namespace ApartmentBook.MVC.Features.Payments.DTOs
{
    public class PaymentForCreateDTO
    {
        public decimal Amount { get; set; }
        public decimal AmountPaid { get; set; }
        public PaymentType Type { get; set; }
        public Guid ApartmentId { get; set; }
        public Guid UserId { get; set; }
    }
}