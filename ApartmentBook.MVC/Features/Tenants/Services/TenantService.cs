using ApartmentBook.MVC.Features.Tenants.Models;
using ApartmentBook.MVC.Features.Tenants.Repositories;

namespace ApartmentBook.MVC.Features.Tenants.Services
{
    public class TenantService : ITenantService
    {
        private readonly ITenantRepository tenantRepository;

        public TenantService(ITenantRepository tenantRepository)
        {
            this.tenantRepository = tenantRepository;
        }

        public async Task<Tenant> GetAsync(Guid? id)
        {
            return await tenantRepository.GetAsync(id);
        }

        public async Task<List<Tenant>> GetUsersTenants(string userId)
        {
            return await tenantRepository.GetUsersTenants(userId);
        }

        public async Task UpdateAsync(Tenant tenant)
        {
            await tenantRepository.UpdateAsync(tenant);
        }

        public async Task CreateAsync(Tenant tenant)
        {
            await tenantRepository.CreateAsync(tenant);
        }

        public async Task DeleteAsync(Guid id)
        {
            var tenant = await tenantRepository.GetAsync(id);
            if (tenant is not null)
            {
                await tenantRepository.DeleteAsync(tenant);
            }
        }
    }
}