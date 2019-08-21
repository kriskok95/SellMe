using System.Threading.Tasks;

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

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await this.categoriesService.GetCategoryViewModelsAsync();
            return this.View(categories);
        }
    }
}
