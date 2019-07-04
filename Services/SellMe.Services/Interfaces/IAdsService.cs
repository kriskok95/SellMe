namespace SellMe.Services.Interfaces
{
    using System.Collections.Generic;
    using SellMe.Web.ViewModels.InputModels.Ads;
    using SellMe.Web.ViewModels.ViewModels.Ads;
    using SellMe.Data.Models;

    public interface IAdsService
    {
        void CreateAd(CreateAdInputModel inputModel);

        AdsAllViewModel GetAllAdViewModels();

        AdsByCategoryViewModel GetAdsByCategoryViewModel(int categoryId);

        AdDetailsViewModel GetAdDetailsViewModel(int adId);

        Ad GetAdById(int adId);

        ICollection<MyActiveAdsViewModel> GetMyAdsViewModels();

        bool ArchiveAdById(int adId);

        ICollection<MyArchivedAdsViewModel> GetMyArchivedAdsViewModels();

        bool ActivateAdById(int adId);
    }
}
