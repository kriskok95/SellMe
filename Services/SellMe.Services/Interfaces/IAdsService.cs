using SellMe.Services.Paging;

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

        Task<AdsByCategoryViewModel> GetAdsByCategoryViewModelAsync(int categoryId, int pageNumber, int pageSize);

        Task<AdDetailsViewModel> GetAdDetailsViewModelAsync(int adId);

        Task<Ad> GetAdByIdAsync(int adId);

        Task<bool> ArchiveAdByIdAsync(int adId);

        Task<bool> ActivateAdByIdAsync(int adId);

        Task<EditAdBindingModel> GetEditAdBindingModelById(int adId);

        Task<string> GetAdTitleByIdAsync(int adId);

        Task UpdateAdByIdAsync(int adId);

        Task<ICollection<PromotedAdViewModel>> GetPromotedAdViewModelsAsync();

        Task<ICollection<LatestAddedAdViewModel>> GetLatestAddedAdViewModelsAsync();

        Task<AdsBySubcategoryViewModel> GetAdsBySubcategoryViewModelAsync(int subcategoryId, int categoryId, int pageNumber, int pageSize);

        Task<PaginatedList<MyActiveAdsViewModel>> GetMyActiveAdsViewModelsAsync(int pageNumber, int pageSize);

        Task<PaginatedList<FavoriteAdViewModel>> GetFavoriteAdsViewModelsAsync(string userId, int pageNumber, int pageSize);

        Task<PaginatedList<MyArchivedAdsViewModel>> GetArchivedAdsViewModelsAsync(int pageNumber, int pageSize);

        Task<PaginatedList<AdViewModel>> GetAdsBySearchViewModelsAsync(string searchText, int pageNumber, int pageSize);

        Task<AdsByUserBindingModel> GetAdsByUserBindingModelAsync(string userId, int pageNumber, int pageSize);

        Task EditAd(EditAdInputModel inputModel);

        Task<PaginatedList<AdForApprovalViewModel>> GetAdsForApprovalViewModelsAsync(int pageNumber, int PageSize);

        Task<bool> ApproveAdAsync(int adId);

        Task<RejectAdBindingModel> GetRejectAdBindingModelAsync(int adId);

        Task CreateAdRejectionAsync(int adId, string comment);

        Task<PaginatedList<WaitingForApprovalByUserViewModel>> GetWaitingForApprovalByCurrentUserViewModels(int pageNumber, int pageSize);

        Task<PaginatedList<RejectedByUserAdViewModel>> GetRejectedAdByUserViewModelsAsync(int pageNumber, int pageSize);

        Task<bool> SubmitRejectedAdAsync(int rejectionId);

        Task<PaginatedList<RejectedAdAllViewModel>> GetRejectedAdAllViewModelsAsync(int pageNumber, int pageSize);

        Task<PaginatedList<ActiveAdAllViewModel>> GetAllActiveAdViewModelsAsync(int pageNumber, int pageSize);

        Task<List<int>> GetTheCountForTheCreatedAdsForTheLastTenDaysAsync();

        Task<int> GetAllActiveAdsCountAsync();
    }
}
