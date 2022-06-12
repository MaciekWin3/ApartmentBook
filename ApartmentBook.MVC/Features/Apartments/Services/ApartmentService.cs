using ApartmentBook.MVC.Features.Apartments.Models;
using ApartmentBook.MVC.Features.Apartments.Repositories;

namespace ApartmentBook.MVC.Features.Apartments.Services
{
    public class ApartmentService : IApartmentService
    {
        private readonly IApartmentRepository apartmentRepository;

        public ApartmentService(IApartmentRepository apartmentRepository)
        {
            this.apartmentRepository = apartmentRepository;
        }

        public async Task<Apartment> GetAsync(Guid? id)
        {
            return await apartmentRepository.GetAsync(id);
        }

        public async Task<List<Apartment>> GetUsersApartments(string userId)
        {
            return await apartmentRepository.GetUsersApartments(userId);
        }

        public async Task<List<Apartment>> GetAllAsync()
        {
            return await apartmentRepository.GetAllAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var apartment = await apartmentRepository.GetAsync(id);
            if (apartment is not null)
            {
                await apartmentRepository.DeleteAsync(apartment);
            }
        }

        public async Task UpdateAsync(Apartment apartment)
        {
            await apartmentRepository.UpdateAsync(apartment);
        }

        public async Task CreateAsync(Apartment apartment)
        {
            await apartmentRepository.CreateAsync(apartment);
        }
    }
}