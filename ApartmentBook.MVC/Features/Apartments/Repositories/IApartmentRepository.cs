using ApartmentBook.MVC.Features.Apartments.Models;

namespace ApartmentBook.MVC.Features.Apartments.Repositories
{
    public interface IApartmentRepository
    {
        Task CreateAsync(Apartment apartment);
        Task<List<Apartment>> GetAllAsync();
        Task<Apartment> GetAsync(Guid? id);
        Task<List<Apartment>> GetUsersApartments(string userId);
        Task DeleteAsync(Apartment apartment);
        Task UpdateAsync(Apartment apartment);
    }
}