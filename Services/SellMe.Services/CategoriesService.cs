namespace SellMe.Services
{
    using System.Threading.Tasks;
    using SellMe.Data;
    using SellMe.Services.Interfaces;
    using System.Linq;
    using SellMe.Data.Models;
    using System.Collections.Generic;
    using SellMe.Services.Mapping;
    using Microsoft.EntityFrameworkCore;
    using SellMe.Web.ViewModels.ViewModels.Categories;

    public class CategoriesService : ICategoriesService
    {
        private readonly SellMeDbContext context;

        public CategoriesService(SellMeDbContext context)
        {
            this.context = context;
        }

        public ICollection<CreateAdCategoryViewModel> GetCategoryViewModels()
        {
            var categoryViewModels = this.context
                .Categories
                .To<CreateAdCategoryViewModel>()
                .ToList();

            return categoryViewModels;
        }

        public async Task<Category> GetCategoryByIdAsync(int categoryId)
        {
            Category category = await this.context
                .Categories
                .FirstOrDefaultAsync(x => x.Id == categoryId);

            return category;
        }

        public async Task<ICollection<CategoryViewModel>> GetAllCategoryViewModelAsync()
        {
            var allCategories = await this.context
                .Categories
                .To<CategoryViewModel>()
                .ToListAsync();

            return allCategories;
        }

        public string GetCategoryNameById(int categoryId)
        {
            var categoryName = this.context.Categories.FirstOrDefault(x => x.Id == categoryId)?.Name;
            return categoryName;
        }
    }
}
