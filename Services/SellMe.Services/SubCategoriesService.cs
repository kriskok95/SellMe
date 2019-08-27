namespace SellMe.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Data;
    using Interfaces;
    using Mapping;
    using Microsoft.EntityFrameworkCore;
    using Web.ViewModels.ViewModels.Subcategories;

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
            if (!context.Categories.Any(x => x.Id == categoryId))
            {
                throw new ArgumentException(InvalidCategoryIdErrorMessage);
            }

            var subcategoryViewModels = await context
                .SubCategories
                .Where(x => x.CategoryId == categoryId)
                .To<CreateAdSubcategoryViewModel>()
                .ToListAsync();

            return subcategoryViewModels;
        }

        public async Task<ICollection<AdsByCategorySubcategoryViewModel>> GetAdsByCategorySubcategoryViewModelsAsync(int categoryId)
        {
            if (!context.Categories.Any(x => x.Id == categoryId))
            {
                throw new ArgumentException(InvalidCategoryIdErrorMessage);
            }

            var subcategoryViewModels = await context
                .SubCategories
                .Where(x => x.CategoryId == categoryId)
                .To<AdsByCategorySubcategoryViewModel>()
                .ToListAsync();

            return subcategoryViewModels;
        }
    }
}
