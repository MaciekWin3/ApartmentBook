using ApartmentBook.MVC.Features.Apartments.Models;
using ApartmentBook.MVC.Features.Auth.Models;
using ApartmentBook.MVC.Features.Payments.Models;

namespace ApartmentBook.MVC.Features.Tenants.Models
{
    public class Tenant
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int RoomatesAmount { get; set; }
        public string Notes { get; set; }
        public virtual List<Payment> Payments { get; set; }
        public virtual Apartment Apartment { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}