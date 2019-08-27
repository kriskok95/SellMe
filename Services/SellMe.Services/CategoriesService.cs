namespace SellMe.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Data;
    using Data.Models;
    using Interfaces;
    using Mapping;
    using Microsoft.EntityFrameworkCore;
    using Web.ViewModels.ViewModels.Categories;

    public class CategoriesService : ICategoriesService
    {
        private const string GetCategoryNameByIdInvalidIdErrorMessage = "Category with the given id doesn't exist!";
        private const string GetCategoryByIdInvalidErrorMessage = "Category with the given Id doesn't exist!";

        private readonly SellMeDbContext context;

        public CategoriesService(SellMeDbContext context)
        {
            this.context = context;
        }

        public async Task<ICollection<CreateAdCategoryViewModel>> GetCategoryViewModelsAsync()
        {
            var categoryViewModels = await context
                .Categories
                .To<CreateAdCategoryViewModel>()
                .ToListAsync();

            return categoryViewModels;
        }

        public async Task<Category> GetCategoryByIdAsync(int categoryId)
        {
            if (!await context.Categories.AnyAsync(x => x.Id == categoryId))
            {
                throw new ArgumentException(GetCategoryByIdInvalidErrorMessage);
            }

            Category category = await context
                .Categories
                .FirstOrDefaultAsync(x => x.Id == categoryId);

            return category;
        }

        public async Task<ICollection<CategoryViewModel>> GetAllCategoryViewModelsAsync()
        {
            var allCategories = await context
                .Categories
                .To<CategoryViewModel>()
                .ToListAsync();

            return allCategories;
        }

        public async Task<string> GetCategoryNameByIdAsync(int categoryId)
        {
            if (! await context.Categories.AnyAsync(x => x.Id == categoryId))
            {
                throw new ArgumentException(GetCategoryNameByIdInvalidIdErrorMessage);
            }

            var categoryName = context
                .Categories
                .FirstOrDefaultAsync(x => x.Id == categoryId)?.Result.Name;
            return categoryName;
        }
    }
}
