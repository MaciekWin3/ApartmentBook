using ApartmentBook.MVC.Data;
using ApartmentBook.MVC.Features.Apartments.Models;
using ApartmentBook.MVC.Features.Auth.Models;

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

        //public DateTime CreationDate { get; set; }
        //public DateTime LastModificationDate { get; set; }
        public virtual Apartment Apartment { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}