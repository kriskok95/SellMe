namespace SellMe.Services
{
    using System;
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
        private const string InvalidCategoryIdErrorMessage = "Category with the given id doesn't exist!";

        private readonly SellMeDbContext context;

        public SubCategoriesService(SellMeDbContext context)
        {
            this.context = context;
        }

        public async Task<ICollection<CreateAdSubcategoryViewModel>> GetSubcategoriesByCategoryIdAsync(int categoryId)
        {
            if (!this.context.Categories.Any(x => x.Id == categoryId))
            {
                throw new ArgumentException(InvalidCategoryIdErrorMessage);
            }

            var subcategoryViewModels = await this.context
                .SubCategories
                .Where(x => x.CategoryId == categoryId)
                .To<CreateAdSubcategoryViewModel>()
                .ToListAsync();

            return subcategoryViewModels;
        }

        public async Task<ICollection<AdsByCategorySubcategoryViewModel>> GetAdsByCategorySubcategoryViewModelsAsync(int categoryId)
        {
            if (!this.context.Categories.Any(x => x.Id == categoryId))
            {
                throw new ArgumentException(InvalidCategoryIdErrorMessage);
            }

            var subcategoryViewModels = await this.context
                .SubCategories
                .Where(x => x.CategoryId == categoryId)
                .To<AdsByCategorySubcategoryViewModel>()
                .ToListAsync();

            return subcategoryViewModels;
        }
    }
}
