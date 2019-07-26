using System.Linq;

namespace SellMe.Services
{
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

        public async Task<IndexViewModel> GetIndexViewModel()
        {
            var categoriesViewModel = await this.categoriesService.GetAllCategoryViewModelAsync();
            var promotedAdViewModel = await this.adsService.GetPromotedAdViewModels();
            var latestAddedAdsViewModel = await this.adsService.GetLatestAddedAdViewModels();

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
