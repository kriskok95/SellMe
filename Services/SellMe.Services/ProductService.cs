﻿namespace SellMe.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using SellMe.Data;
    using SellMe.Services.Interfaces;
    using Microsoft.EntityFrameworkCore;
    using SellMe.Data.Models;
    using SellMe.Services.Utilities;
    using SellMe.Web.ViewModels.InputModels.Products;

    public class ProductService : IProductService
    {
        private readonly SellMeDbContext context;

        public ProductService(SellMeDbContext context)
        {
            this.context = context;
        }

        public ICollection<string> GetCategoryNames()
        {
            var categoryNames = this.context
                .Categories
                .Select(x => x.Name)
                .ToList();

            return categoryNames;
        }

        public ICollection<SubCategory> GetSubcategoriesByCategory(string categoryName)
        {
            Category parentCategory = this.GetCategoryByName(categoryName);

            return parentCategory.SubCategories.ToList();
        }

        public ICollection<string> GetConditionsFromDb()
        {
            var conditionsFromDb = this.context
                .Conditions
                .Select(x => x.Name)
                .ToList();

            return conditionsFromDb;
        }

        public void CreateProduct(CreateProductInputModel inputModel)
        {
            var cloudinary = CloudinaryHelper.SetCloudinary();
        }


        private Category GetCategoryByName(string categoryName)
        {
            Category category = this.context
                .Categories
                .Include(x => x.SubCategories)
                .FirstOrDefault(x => x.Name == categoryName);

            return category;
        }
    }
}
