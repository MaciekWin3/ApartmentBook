using ApartmentBook.MVC.Data;
using ApartmentBook.MVC.Features.Tenants.Models;

namespace ApartmentBook.MVC.Features.Tenants.Repositories
{
    public class TenantRepository : BaseRepository
    {
        public TenantRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task CreateAsync(Tenant tenant)
        {
            await context.Tenants.AddAsync(tenant);
            await context.SaveChangesAsync();
        }
    }
}