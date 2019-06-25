namespace SellMe.Services
{
    using SellMe.Data;
    using SellMe.Services.Interfaces;
    using System.Linq;
    using SellMe.Data.Models;

    public class CategoriesService : ICategoriesService
    {
        private readonly SellMeDbContext context;

        public CategoriesService(SellMeDbContext context)
        {
            this.context = context;
        }

        public int GetCategoryIdByName(string categoryName)
        {
            int categoryId = this.context
                .Categories
                .FirstOrDefault(x => x.Name == categoryName).Id;

            return categoryId;
        }
    }
}
