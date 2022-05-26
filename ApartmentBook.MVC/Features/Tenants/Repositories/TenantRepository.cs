using ApartmentBook.MVC.Data;

namespace ApartmentBook.MVC.Features.Tenants.Repositories
{
    public class TenantRepository : BaseRepository
    {
        protected TenantRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
