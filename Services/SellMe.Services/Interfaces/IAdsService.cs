﻿namespace SellMe.Services.Interfaces
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

        Task<AdsAllViewModel> GetAllAdViewModelsAsync(int pageNumber, int pageSize);

        Task<AdsByCategoryViewModel> GetAdsByCategoryViewModelAsync(int categoryId, int pageNumber, int pageSize);

        Task<AdDetailsViewModel> GetAdDetailsViewModelAsync(int adId);

        Task<Ad> GetAdByIdAsync(int adId);

        Task<bool> ArchiveAdByIdAsync(int adId);

        Task<bool> ActivateAdById(int adId);

        Task<EditAdBindingModel> GetEditAdBindingModelById(int adId);

        string GetAdTitleById(int adId);

        Task UpdateAdByIdAsync(int adId);

        Task<ICollection<PromotedAdViewModel>> GetPromotedAdViewModels();

        Task<ICollection<LatestAddedAdViewModel>> GetLatestAddedAdViewModels();

        Task<AdsBySubcategoryViewModel> GetAdsBySubcategoryViewModelAsync(int subcategoryId, int categoryId, int pageNumber, int pageSize);

        Task<MyActiveAdsBindingModel> GetMyActiveAdsBindingModelAsync(int pageNumber, int pageSize);

        Task<FavoriteAdsBindingModel> GetFavoriteAdsBindingModelAsync(string userId, int pageNumber, int pageSize);

        Task<ArchivedAdsBindingModel> GetArchivedAdsBindingModelAsync(int pageNumber, int pageSize);

        Task<AdsBySearchViewModel> GetAdsBySearchViewModelAsync(string searchText);

        Task<AdsByUserBindingModel> GetAdsByUserBindingModelAsync(string userId);

        Task EditAdById(EditAdInputModel inputModel);

        Task<AdsForApprovalViewModel> GetAdsForApprovalViewModelsAsync();

        Task<bool> ApproveAdAsync(int adId);

        Task<object> GetRejectAdBindingModelAsync(int adId);

        Task CreateAdRejectionAsync(int adId, string comment);

        Task<ICollection<WaitingForApprovalByUserViewModel>> GetWaitingForApprovalByCurrentUserViewModels();

        Task<ICollection<RejectedByUserAdViewModel>> GetRejectedAdByUserViewModelsAsync();

        Task<bool> SubmitRejectedAdAsync(int rejectionId);

        Task<ICollection<RejectedAdAllViewModel>> GetRejectedAdAllViewModelsAsync();

        Task<IEnumerable<ActiveAdAllViewModel>> GetAllActiveAdViewModelsAsync();

        Task<List<int>> GetTheCountForTheCreatedAdsForTheLastTenDays();
    }
}
