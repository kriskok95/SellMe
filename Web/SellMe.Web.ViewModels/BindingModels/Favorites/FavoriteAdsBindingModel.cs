namespace SellMe.Web.ViewModels.BindingModels.Favorites
{
    using SellMe.Web.ViewModels.ViewModels;
    using System.Collections.Generic;
    using SellMe.Web.ViewModels.ViewModels.Ads;
    public class FavoriteAdsBindingModel : BaseViewModel
    {
        public IEnumerable<FavoriteAdViewModel> Favorites { get; set; }
    }
}
