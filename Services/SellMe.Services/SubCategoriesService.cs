namespace SellMe.Services
{
    using System.Linq;
    using SellMe.Data;
    using SellMe.Data.Models;
    using SellMe.Services.Interfaces;

    public class SubCategoriesService : ISubCategoriesService
    {
        private readonly SellMeDbContext context;

        public SubCategoriesService(SellMeDbContext context)
        {
            this.context = context;
        }

        public int GetSubCategoryIdByName(string subCategoryName)
        {
            //TODO: Validate for null reference exception
            int subCategoryId = this.context
                .SubCategories
                .FirstOrDefault(x => x.Name == subCategoryName).Id;

            return subCategoryId;
        }
    }
}
