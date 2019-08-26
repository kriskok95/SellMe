namespace SellMe.Services
{
    using System.Linq;

    using SellMe.Services.Interfaces;
    using SellMe.Web.ViewModels.ViewModels.Home;
    using System.Threading.Tasks;

    public class HomeService : IHomeService
    {
        private readonly ICategoriesService categoriesService;
        private readonly IAdsService adsService;

        public HomeService(ICategoriesService categoriesService, IAdsService adsService)
        {
            this.categoriesService = categoriesService;
            this.adsService = adsService;
        }

        public async Task<IndexViewModel> GetIndexViewModelAsync()
        {
            var categoriesViewModel = await this.categoriesService.GetAllCategoryViewModelsAsync();
            var promotedAdViewModel = await this.adsService.GetPromotedAdViewModelsAsync();
            var latestAddedAdsViewModel = await this.adsService.GetLatestAddedAdViewModelsAsync();

            var indexViewModel = new IndexViewModel()
            {
                CategoryViewModels = categoriesViewModel.ToList(),
                PromotedAdViewModels = promotedAdViewModel.ToList(),
                LatestAddedAdViewModels = latestAddedAdsViewModel.ToList(),
            };

            return indexViewModel;
        }
    }
}
