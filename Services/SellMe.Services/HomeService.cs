using System.Linq;

namespace SellMe.Services
{
    using SellMe.Services.Interfaces;
    using SellMe.Web.ViewModels.ViewModels.Home;
    using System.Threading.Tasks;

    public class HomeService : IHomeService
    {
        private readonly ICategoriesService categoriesService;

        public HomeService(ICategoriesService categoriesService)
        {
            this.categoriesService = categoriesService;
        }

        public async Task<IndexViewModel> GetIndexViewModel()
        {
            var categoriesViewModel = await this.categoriesService.GetAllCategoryViewModelAsync();

            var indexViewModel = new IndexViewModel()
            {
                CategoryViewModels = categoriesViewModel.ToList()
            };

            return indexViewModel;
        }
    }
}
