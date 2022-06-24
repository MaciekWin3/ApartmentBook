using ApartmentBook.MVC.Features.Tenants.Models;

namespace ApartmentBook.MVC.Features.Tenants.Services
{
    public interface ITenantService
    {
        Task CreateAsync(Tenant tenant);
        Task DeleteAsync(Guid id);
        Task<Tenant> GetAsync(Guid? id);
        Task<List<Tenant>> GetUsersTenants(string userId);
        Task UpdateAsync(Tenant tenant);
    }
}