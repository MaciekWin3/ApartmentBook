using ApartmentBook.MVC.Features.Apartments.Models;
using ApartmentBook.MVC.Features.Payments.Models;

namespace ApartmentBook.MVC.Features.Tenants.Models
{
    public class Tenant
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public virtual Apartment Apartment { get; set; }
        public IList<Payment> Payments { get; set; }
    }
}
