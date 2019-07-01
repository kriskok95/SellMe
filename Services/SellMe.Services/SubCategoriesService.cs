﻿namespace SellMe.Services
{
    using System.Linq;
    using SellMe.Data;
    using SellMe.Data.Models;
    using SellMe.Services.Interfaces;
    using System.Collections.Generic;
    using SellMe.Services.Mapping;
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

        public int GetSubCategoryIdByName(string subCategoryName)
        {
            //TODO: Validate for null reference exception
            int subCategoryId = this.context
                .SubCategories
                .FirstOrDefault(x => x.Name == subCategoryName).Id;

            return subCategoryId;
        }

        public ICollection<CreateAdSubcategoryViewModel> GetSubcategoriesByCategoryId(int categoryId)
        {
            Category category = this.categoryService.GetCategoryById(categoryId);

            var subcategoryViewModels = category
                .SubCategories
                .AsQueryable()
                .To<CreateAdSubcategoryViewModel>()
                .ToList();

            return subcategoryViewModels;
        }
    }
}
