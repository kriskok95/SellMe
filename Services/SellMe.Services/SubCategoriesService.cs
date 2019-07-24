namespace SellMe.Services
{
    using System.Linq;
    using SellMe.Data;
    using SellMe.Data.Models;
    using SellMe.Services.Interfaces;
    using System.Collections.Generic;
    using SellMe.Services.Mapping;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using SellMe.Web.ViewModels.ViewModels.Subcategories;

    public class SubCategoriesService : ISubCategoriesService
    {
        private readonly SellMeDbContext context;
        private readonly ICategoriesService categoryService;

        public SubCategoriesService(SellMeDbContext context, ICategoriesService categoryService)
        {
            this.context = context;
            this.categoryService = categoryService;
        }

        public async Task<ICollection<CreateAdSubcategoryViewModel>> GetSubcategoriesByCategoryIdAsync(int categoryId)
        {
            Category category = await this.categoryService.GetCategoryByIdAsync(categoryId);

            var subcategoryViewModels = category
                .SubCategories
                .AsQueryable()
                .To<CreateAdSubcategoryViewModel>()
                .ToList();

            return subcategoryViewModels;
        }

        public async Task<ICollection<AdsByCategorySubcategoryViewModel>> GetAdsByCategorySubcategoryViewModelsAsync(int categoryId)
        {
            var subcategoryViewModels = await this.context
                .SubCategories
                .Where(x => x.CategoryId == categoryId)
                .To<AdsByCategorySubcategoryViewModel>()
                .ToListAsync();

            return subcategoryViewModels;
        }
    }
}
