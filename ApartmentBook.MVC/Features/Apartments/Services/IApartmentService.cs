using ApartmentBook.MVC.Features.Apartments.Models;

namespace ApartmentBook.MVC.Features.Apartments.Services
{
    public interface IApartmentService
    {
        Task CreateAsync(Apartment apartment);

        Task DeleteAsync(Guid id);

        Task<Apartment> GetAsync(Guid? id);

        Task<List<Apartment>> GetUsersApartments(string userId);

        Task UpdateAsync(Apartment apartment);

        Task<List<Apartment>> GetAllAsync();
    }
}