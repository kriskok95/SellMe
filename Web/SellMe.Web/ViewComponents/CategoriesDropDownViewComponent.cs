namespace SellMe.Web.ViewComponents
{
    using Microsoft.AspNetCore.Mvc;
    using SellMe.Services.Interfaces;

    public class CategoriesDropdownViewComponent : ViewComponent
    {
        private readonly ICategoriesService categoriesService;

        public CategoriesDropdownViewComponent(ICategoriesService categoriesService)
        {
            this.categoriesService = categoriesService;
        }

        public IViewComponentResult Invoke()
        {
            var categories = this.categoriesService.GetCategoryViewModels();
            return this.View(categories);
        }
    }
}
