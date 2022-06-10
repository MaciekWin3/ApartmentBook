using ApartmentBook.MVC.Data;
using ApartmentBook.MVC.Features.Payments.Models;
using Microsoft.EntityFrameworkCore;

namespace ApartmentBook.MVC.Features.Payments.Repositories
{
    public class PaymentRepository : BaseRepository, IPaymentRepository
    {
        public PaymentRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Payment> GetAsync(Guid? id)
        {
            return await context.Payments
                .Include(p => p.Apartment)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Payment>> GetApartmentPayments(Guid apartmentId)
        {
            return await context.Payments
                .Where(p => p.Apartment.Id == apartmentId)
                .ToListAsync();
        }

        public async Task<List<Payment>> GetUsersPayments(string userId)
        {
            return await context.Payments
                .Where(p => p.Apartment.User.Id == userId)
                .ToListAsync();
        }

        public async Task CreateAsync(Payment payment)
        {
            await context.Payments.AddAsync(payment);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Payment payment)
        {
            context.Payments.Update(payment);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Payment payment)
        {
            context.Payments.Remove(payment);
            await context.SaveChangesAsync();
        }
    }
}