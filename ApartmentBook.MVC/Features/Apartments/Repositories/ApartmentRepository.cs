using ApartmentBook.MVC.Data;
using ApartmentBook.MVC.Features.Apartments.Models;
using Microsoft.EntityFrameworkCore;

namespace ApartmentBook.MVC.Features.Apartments.Repositories
{
    public class ApartmentRepository : BaseRepository, IApartmentRepository
    {
        public ApartmentRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Apartment> GetAsync(Guid? id)
        {
            return await context.Apartments.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<List<Apartment>> GetUsersApartments(string userId)
        {
            return await context.Apartments
                .Where(a => a.User.Id == userId)
                .ToListAsync();
        }

        public async Task<List<Apartment>> GetAllAsync()
        {
            return await context.Apartments.ToListAsync();
        }

        public async Task CreateAsync(Apartment apartment)
        {
            await context.Apartments.AddAsync(apartment);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Apartment apartment)
        {
            context.Apartments.Update(apartment);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Apartment apartment)
        {
            context.Apartments.Remove(apartment);
            await context.SaveChangesAsync();
        }
    }
}
