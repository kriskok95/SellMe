using SellMe.Web.ViewModels.ViewModels.Products;

namespace SellMe.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using SellMe.Data;
    using SellMe.Services.Interfaces;
    using Microsoft.EntityFrameworkCore;
    using SellMe.Data.Models;
    using SellMe.Services.Utilities;
    using SellMe.Web.ViewModels.InputModels.Products;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using SellMe.Services.Mapping;

    public class ProductsService : IProductsService
    {
        private readonly SellMeDbContext context;
        private readonly ICategoriesService categoryService;
        private readonly ISubCategoriesService subCategoriesService;
        private readonly IConditionsService conditionsService;

        public ProductsService(SellMeDbContext context, ICategoriesService categoryService, ISubCategoriesService subCategoriesService, IConditionsService conditionsService)
        {
            this.context = context;
            this.categoryService = categoryService;
            this.subCategoriesService = subCategoriesService;
            this.conditionsService = conditionsService;
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
            //Export this into separate method
            var imageUrls = inputModel.Images
                .Select(x => this.UploadImages(x, inputModel.Title))
                .ToList();

            //var product = inputModel.

            //TODO: Implement model mapper!
            var product = new Product
            {
                Title = inputModel.Title,
                CategoryId = categoryService.GetCategoryIdByName(inputModel.Category),
                SubCategoryId = this.subCategoriesService.GetSubCategoryIdByName(inputModel.SubCategory),
                Description = inputModel.Description,
                AvailabilityCount = inputModel.Availability,
                Condition = this.conditionsService.GetConditionByName(inputModel.Condition),
                Images = imageUrls.Select(x => new Image { ImageUrl = x.Result }).ToList(),
                CreatedOn = DateTime.UtcNow
            };

            this.context.Products.Add(product);
            this.context.SaveChanges();
        }

        public ICollection<ProductsAllViewModel> GetAllProductsViewModels()
        {
            var allProductsViewModel = this.context
                .Products
                .Include(x => x.Category)
                .Include(x => x.SubCategory)
                .Include(x => x.Images)
                .To<ProductsAllViewModel>()
                .ToList();

            return allProductsViewModel;
        }

        private async Task<string> UploadImages(IFormFile inputModelImage, string title)
        {
            var cloudinary = CloudinaryHelper.SetCloudinary();

            var url = await CloudinaryHelper.UploadImage(cloudinary, inputModelImage, title);

            return url;
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
