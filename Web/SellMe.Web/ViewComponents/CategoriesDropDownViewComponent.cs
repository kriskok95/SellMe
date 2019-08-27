namespace SellMe.Web.ViewComponents
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Services.Interfaces;

    public class CategoriesDropdownViewComponent : ViewComponent
    {
        private readonly ICategoriesService categoriesService;

        public CategoriesDropdownViewComponent(ICategoriesService categoriesService)
        {
            this.categoriesService = categoriesService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await categoriesService.GetCategoryViewModelsAsync();
            return View(categories);
        }
    }
}
