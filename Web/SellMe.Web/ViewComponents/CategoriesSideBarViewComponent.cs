namespace SellMe.Web.ViewComponents
{
    using Microsoft.AspNetCore.Mvc;
    using SellMe.Services.Interfaces;
    using System.Linq;
    using System.Threading.Tasks;

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
                .GetAllCategoryViewModelsAsync()
                .GetAwaiter()
                .GetResult()
                .ToList();

            return this.View(categories);
        }
    }
}
