using ApartmentBook.MVC.Features.Tenants.Models;

namespace ApartmentBook.MVC.Features.Tenants.Repositories
{
    public interface ITenantRepository
    {
        Task CreateAsync(Tenant tenant);

        Task DeleteAsync(Tenant tenant);

        Task<Tenant> GetAsync(Guid? id);

        Task<List<Tenant>> GetUsersTenants(string userId);

        Task UpdateAsync(Tenant tenant);
    }
}