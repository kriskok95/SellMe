namespace SellMe.Services
{
    using SellMe.Data;
    using SellMe.Services.Interfaces;

    public class CategoriesService : ICategoriesService
    {
        private readonly SellMeDbContext context;

        public CategoriesService(SellMeDbContext context)
        {
            this.context = context;
        }

        public bool CreateCategory()
        {
            return true;
        }
    }
}
