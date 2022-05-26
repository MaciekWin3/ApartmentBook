namespace ApartmentBook.MVC.Data
{
    public class BaseRepository
    {
        protected readonly ApplicationDbContext context;
        protected BaseRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
    }
}
