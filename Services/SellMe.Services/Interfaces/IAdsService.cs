namespace SellMe.Services.Interfaces
{
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using SellMe.Web.ViewModels.InputModels.Ads;
    using SellMe.Web.ViewModels.ViewModels.Ads;
    using SellMe.Data.Models;
    using SellMe.Web.ViewModels.BindingModels.Ads;

    public interface IAdsService
    {
        Task CreateAdAsync(CreateAdInputModel inputModel);

        Task<AdsAllViewModel> GetAllAdViewModelsAsync();

        Task<AdsByCategoryViewModel> GetAdsByCategoryViewModelAsync(int categoryId);

        Task<AdDetailsViewModel> GetAdDetailsViewModelAsync(int adId);

        Ad GetAdById(int adId);

        Task<ICollection<MyActiveAdsViewModel>> GetMyAdsViewModelsAsync();

        Task<bool> ArchiveAdByIdAsync(int adId);

        Task<bool> ActivateAdById(int adId);

        Task<ICollection<MyArchivedAdsViewModel>> GetMyArchivedAdsViewModelsAsync();

        EditAdBindingModel GetEditAdBindingModelById(int adId);

        Task<ICollection<FavoriteAdViewModel>> GetFavoriteAdsByUserIdAsync(string loggedInUserId);

        string GetAdTitleById(int adId);
    }
}
