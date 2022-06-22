using ApartmentBook.MVC.Features.Apartments.Models;
using ApartmentBook.MVC.Features.Payments.Models;
using ApartmentBook.MVC.Features.Tenants.Models;
using Microsoft.AspNetCore.Identity;

namespace ApartmentBook.MVC.Features.Auth.Models
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<Apartment> Apartments { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
        public virtual ICollection<Tenant> Tenants { get; set; }
    }
}