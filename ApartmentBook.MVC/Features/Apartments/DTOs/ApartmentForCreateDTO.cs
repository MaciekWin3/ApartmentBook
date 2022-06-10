using System.ComponentModel.DataAnnotations;

namespace ApartmentBook.MVC.Features.Apartments.DTOs
{
    public class ApartmentForCreateDTO
    {
        public string Name { get; set; }
        public string Street { get; set; }
        public string Building { get; set; }
        public string Flat { get; set; }
        public string PostCode { get; set; }
        public int Meterage { get; set; }
        public decimal Value { get; set; }

        [DataType(DataType.Currency)]
        public decimal Rent { get; set; }

        public string UserId { get; set; }
    }
}