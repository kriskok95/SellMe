using System.Security.Claims;

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
    using Microsoft.AspNetCore.Identity;
    using SellMe.Web.ViewModels.ViewModels.Products;

    public class ProductsService : IProductsService
    {
        private readonly SellMeDbContext context;
        private readonly ICategoriesService categoryService;
        private readonly ISubCategoriesService subCategoriesService;
        private readonly IConditionsService conditionsService;
        private readonly IHttpContextAccessor contextAccessor;

        public ProductsService(SellMeDbContext context, ICategoriesService categoryService, ISubCategoriesService subCategoriesService, IConditionsService conditionsService, IHttpContextAccessor contextAccessor)
        {
            this.context = context;
            this.categoryService = categoryService;
            this.subCategoriesService = subCategoriesService;
            this.conditionsService = conditionsService;
            this.contextAccessor = contextAccessor;
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

            var username = this.contextAccessor.HttpContext.User.Identity.Name;

            //TODO: Implement model mapper!
            var product = new Ad
            {
                SellerId = this.contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value,
                Title = inputModel.Title,
                CategoryId = categoryService.GetCategoryIdByName(inputModel.Category),
                SubCategoryId = this.subCategoriesService.GetSubCategoryIdByName(inputModel.SubCategory),
                Description = inputModel.Description,
                AvailabilityCount = inputModel.Availability,
                Condition = this.conditionsService.GetConditionByName(inputModel.Condition),
                Images = imageUrls.Select(x => new Image { ImageUrl = x.Result }).ToList(),
                CreatedOn = DateTime.UtcNow, 
                Price = inputModel.Price
            };

            this.context.Ads.Add(product);
            this.context.SaveChanges();
        }

        public ICollection<AdsAllViewModel> GetAllProductsViewModels()
        {
            var allProductsViewModel = this.context
                .Ads
                .Include(x => x.Category)
                .Include(x => x.SubCategory)
                .Include(x => x.Images)
                .To<AdsAllViewModel>()
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
