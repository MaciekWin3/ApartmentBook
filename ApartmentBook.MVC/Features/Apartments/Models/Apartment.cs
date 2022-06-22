using ApartmentBook.MVC.Data;
using ApartmentBook.MVC.Features.Auth.Models;
using ApartmentBook.MVC.Features.Payments.Models;
using ApartmentBook.MVC.Features.Tenants.Models;
using System.ComponentModel.DataAnnotations;

namespace ApartmentBook.MVC.Features.Apartments.Models
{
    public class Apartment : BaseModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Building { get; set; }
        public string Flat { get; set; }
        public int Meterage { get; set; }
        public string PostCode { get; set; }
        public decimal Value { get; set; }

        [DataType(DataType.Currency)]
        public decimal Rent { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual IList<Payment> Payments { get; set; }
        public virtual IList<Tenant> Tenant { get; set; }
        public string TenantEmail { get; set; }
        //public IList<string> Notes { get; set; }
    }
}