using ApartmentBook.MVC.Features.Tenants.Models;

namespace ApartmentBook.MVC.Features.Payments.Models
{
    public class Payment
    {
        public Guid Id { get; set; }
        public string PaymentType { get; set; }
        public decimal Amount { get; set; }
        public decimal AmountPaid { get ; set; }
        public bool IsPaid { get { return AmountPaid > Amount; } }
        public virtual Tenant Tenant { get; set; }

        //public List<Attat> { get; set; }
    }
}
