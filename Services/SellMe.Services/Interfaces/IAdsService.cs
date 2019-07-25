namespace SellMe.Services.Interfaces
{
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using SellMe.Web.ViewModels.InputModels.Ads;
    using SellMe.Web.ViewModels.ViewModels.Ads;
    using SellMe.Data.Models;
    using SellMe.Web.ViewModels.BindingModels.Ads;
    using SellMe.Web.ViewModels.BindingModels.Favorites;

    public interface IAdsService
    {
        Task CreateAdAsync(CreateAdInputModel inputModel);

        Task<AdsAllViewModel> GetAllAdViewModelsAsync();

        Task<AdsByCategoryViewModel> GetAdsByCategoryViewModelAsync(int categoryId);

        Task<AdDetailsViewModel> GetAdDetailsViewModelAsync(int adId);

        Task<Ad> GetAdByIdAsync(int adId);

        Task<bool> ArchiveAdByIdAsync(int adId);

        Task<bool> ActivateAdById(int adId);

        EditAdBindingModel GetEditAdBindingModelById(int adId);

        string GetAdTitleById(int adId);

        Task UpdateAdByIdAsync(int adId);

        Task<ICollection<PromotedAdViewModel>> GetPromotedAdViewModels();

        Task<ICollection<LatestAddedAdViewModel>> GetLatestAddedAdViewModels();

        Task<AdsBySubcategoryViewModel> GetAdsBySubcategoryViewModelAsync(int subcategoryId, int categoryId);

        Task<MyActiveAdsBindingModel> GetMyActiveAdsBindingModelAsync();

        Task<FavoriteAdsBindingModel> GetFavoriteAdsBindingModelAsync(string userId);

        Task<ArchivedAdsBindingModel> GetArchivedAdsBindingModelAsync();
    }
}
