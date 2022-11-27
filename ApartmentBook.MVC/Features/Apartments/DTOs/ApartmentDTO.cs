using System.ComponentModel.DataAnnotations;

namespace ApartmentBook.MVC.Features.Apartments.DTOs
{
    public record ApartmentDTO
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
        public string TenantEmail { get; set; }
    }
}