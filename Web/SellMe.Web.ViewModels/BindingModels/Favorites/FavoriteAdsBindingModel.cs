namespace SellMe.Web.ViewModels.BindingModels.Favorites
{
    using SellMe.Web.ViewModels.ViewModels;
    using SellMe.Web.ViewModels.ViewModels.Ads;
    using SellMe.Services.Paging;
    public class FavoriteAdsBindingModel : BaseViewModel
    {
        public PaginatedList<FavoriteAdViewModel> Favorites { get; set; }
    }
}
