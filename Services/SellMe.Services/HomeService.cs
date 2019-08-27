namespace SellMe.Services
{
    using System.Linq;
    using System.Threading.Tasks;
    using Interfaces;
    using Web.ViewModels.ViewModels.Home;

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
            var categoriesViewModel = await categoriesService.GetAllCategoryViewModelsAsync();
            var promotedAdViewModel = await adsService.GetPromotedAdViewModelsAsync();
            var latestAddedAdsViewModel = await adsService.GetLatestAddedAdViewModelsAsync();

            var indexViewModel = new IndexViewModel
            {
                CategoryViewModels = categoriesViewModel.ToList(),
                PromotedAdViewModels = promotedAdViewModel.ToList(),
                LatestAddedAdViewModels = latestAddedAdsViewModel.ToList()
            };

            return indexViewModel;
        }
    }
}
