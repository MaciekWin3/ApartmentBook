using ApartmentBook.MVC.Features.Auth.Models;
using ApartmentBook.MVC.Features.Payments.Models;
using ApartmentBook.MVC.Features.Tenants.Models;

namespace ApartmentBook.MVC.Features.Apartments.Models
{
    public class Apartment
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Street { get; set; }
        public string Building { get; set; }
        public string Flat { get; set; }
        public decimal Rent { get; set; }
        public List<Payment> Payments { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual Tenant Tenant { get; set; }
    }
}
