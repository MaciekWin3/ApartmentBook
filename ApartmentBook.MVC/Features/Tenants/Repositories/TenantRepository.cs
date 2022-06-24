using ApartmentBook.MVC.Data;
using ApartmentBook.MVC.Features.Tenants.Models;
using Microsoft.EntityFrameworkCore;

namespace ApartmentBook.MVC.Features.Tenants.Repositories
{
    public class TenantRepository : BaseRepository, ITenantRepository
    {
        public TenantRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Tenant> GetAsync(Guid? id)
        {
            return await context.Tenants
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task CreateAsync(Tenant tenant)
        {
            await context.Tenants.AddAsync(tenant);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Tenant tenant)
        {
            context.Tenants.Update(tenant);
            await context.SaveChangesAsync();
        }

        public async Task<List<Tenant>> GetUsersTenants(string userId)
        {
            // Include apartment???
            return await context.Tenants
                .Where(t => t.User.Id == userId)
                .ToListAsync();
        }

        public async Task DeleteAsync(Tenant tenant)
        {
            context.Tenants.Remove(tenant);
            await context.SaveChangesAsync();
        }
    }
}