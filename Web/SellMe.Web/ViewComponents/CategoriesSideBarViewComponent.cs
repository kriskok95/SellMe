namespace SellMe.Web.ViewComponents
{
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using Services.Interfaces;

    public class CategoriesSideBarViewComponent : ViewComponent
    {
        private readonly ICategoriesService categoriesService;

        public CategoriesSideBarViewComponent(ICategoriesService categoriesService)
        {
            this.categoriesService = categoriesService;
        }

        public IViewComponentResult Invoke()
        {
            var categories = categoriesService
                .GetAllCategoryViewModelsAsync()
                .GetAwaiter()
                .GetResult()
                .ToList();

            return View(categories);
        }
    }
}
