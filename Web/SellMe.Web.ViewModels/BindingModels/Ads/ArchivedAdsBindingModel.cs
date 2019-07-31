namespace SellMe.Web.ViewModels.BindingModels.Ads
{
    using SellMe.Web.ViewModels.ViewModels;
    using SellMe.Web.ViewModels.ViewModels.Ads;
    using SellMe.Services.Paging;
    public class ArchivedAdsBindingModel : BaseViewModel
    {
        public PaginatedList<MyArchivedAdsViewModel> Ads { get; set; }
    }
}
