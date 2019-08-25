namespace SellMe.Web.ViewModels.BindingModels.Ads
{
    using SellMe.Services.Paging;
    using SellMe.Web.ViewModels.ViewModels.Ads;
    using SellMe.Web.ViewModels.ViewModels;

    public class AdsByUserBindingModel : BaseViewModel
    {
        public string UserId { get; set; }

        public string Username { get; set; }

        public PaginatedList<AdViewModel> AdViewModels { get; set; }
    }
}
