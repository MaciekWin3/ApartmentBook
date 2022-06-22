using ApartmentBook.MVC.Data;
using ApartmentBook.MVC.Features.Apartments.Models;
using ApartmentBook.MVC.Features.Auth.Models;
using ApartmentBook.MVC.Features.Tenants.Models;

namespace ApartmentBook.MVC.Features.Payments.Models
{
    public class Payment : BaseModel
    {
        public Guid Id { get; set; }
        public PaymentType Type { get; set; }
        public decimal Amount { get; set; }
        public decimal AmountPaid { get; set; }

        public bool IsPaid
        { get { return AmountPaid >= Amount; } }

        public Month PaymentMonth { get; set; }
        public int PaymentYear { get; set; }

        public virtual Apartment Apartment { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual Tenant Tenant { get; set; }
    }
}