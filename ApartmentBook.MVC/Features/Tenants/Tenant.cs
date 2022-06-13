namespace ApartmentBook.MVC.Features.Tenants
{
    public class Tenant
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int RoomatesAmount { get; set; }
    }
}