namespace SellMe.Services.Interfaces
{
    using System.Collections.Generic;
    using SellMe.Web.ViewModels.InputModels.Ads;
    using SellMe.Web.ViewModels.ViewModels.Ads;

    public interface IAdsService
    {
        ICollection<string> GetConditionsFromDb();

        void CreateAd(CreateAdInputModel inputModel);

        AdsAllViewModel GetAllAdViewModels();

        AdsByCategoryViewModel GetAdsByCategoryViewModel(int categoryId);

        AdDetailsViewModel GetAdDetailsViewModel(int adId);
    }
}
