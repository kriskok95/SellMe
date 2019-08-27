namespace SellMe.Web.ViewModels.ViewModels.Ads
{
    using Services.Paging;

    public class AdsAllViewModel : BaseViewModel
    {
        public PaginatedList<AdViewModel> AdsViewModels { get; set; }
    }
}
