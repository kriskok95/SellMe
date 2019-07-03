namespace SellMe.Services
{
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

        public  Category GetCategoryById(int categoryId)
        {
            Category category = this.context
                .Categories
                .FirstOrDefault(x => x.Id == categoryId);

            return category;
        }
    }
}
