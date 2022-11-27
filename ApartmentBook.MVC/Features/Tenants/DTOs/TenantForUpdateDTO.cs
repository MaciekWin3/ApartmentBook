namespace ApartmentBook.MVC.Features.Tenants.DTOs
{
    public record TenantForUpdateDTO
    {
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int RoomatesAmount { get; set; }
        public string Notes { get; set; }
    }
}