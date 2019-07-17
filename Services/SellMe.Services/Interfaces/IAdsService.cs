using System.Threading.Tasks;

namespace SellMe.Services.Interfaces
{
    using System.Collections.Generic;
    using SellMe.Web.ViewModels.InputModels.Ads;
    using SellMe.Web.ViewModels.ViewModels.Ads;
    using SellMe.Data.Models;
    using SellMe.Web.ViewModels.BindingModels.Ads;

    public interface IAdsService
    {
        void CreateAd(CreateAdInputModel inputModel);

        AdsAllViewModel GetAllAdViewModels();

        AdsByCategoryViewModel GetAdsByCategoryViewModel(int categoryId);

        Task<AdDetailsViewModel> GetAdDetailsViewModel(int adId);

        Ad GetAdById(int adId);

        ICollection<MyActiveAdsViewModel> GetMyAdsViewModels();

        bool ArchiveAdById(int adId);

        ICollection<MyArchivedAdsViewModel> GetMyArchivedAdsViewModels();

        bool ActivateAdById(int adId);

        EditAdBindingModel GetEditAdBindingModelById(int adId);

        Task<ICollection<FavoriteAdViewModel>> GetFavoriteAdsByUserIdAsync(string loggedInUserId);

        string GetAdTitleById(int adId);
    }
}
