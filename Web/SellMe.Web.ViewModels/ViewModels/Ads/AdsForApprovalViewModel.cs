namespace SellMe.Web.ViewModels.ViewModels.Ads
{
    using SellMe.Services.Paging;

    public class AdsForApprovalViewModel : BaseViewModel
    {
        public PaginatedList<AdForApprovalViewModel> AdsAdForApprovalViewModels { get; set; }

    }
}
