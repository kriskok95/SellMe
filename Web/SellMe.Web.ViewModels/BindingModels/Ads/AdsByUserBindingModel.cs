namespace SellMe.Web.ViewModels.BindingModels.Ads
{
    using Services.Paging;
    using ViewModels;
    using ViewModels.Ads;

    public class AdsByUserBindingModel : BaseViewModel
    {
        public string UserId { get; set; }

        public string Username { get; set; }

        public PaginatedList<AdViewModel> AdViewModels { get; set; }
    }
}
