using System.Linq;

namespace SellMe.Web.ViewComponents
{
    using Microsoft.AspNetCore.Mvc;
    using SellMe.Services.Interfaces;

    public class CategoriesSideBarViewComponent : ViewComponent
    {
        private readonly ICategoriesService categoriesService;

        public CategoriesSideBarViewComponent(ICategoriesService categoriesService)
        {
            this.categoriesService = categoriesService;
        }

        public IViewComponentResult Invoke()
        {
            var categories = this.categoriesService
                .GetAllCategoryViewModel()
                .ToList();

            return this.View(categories);
        }
    }
}
