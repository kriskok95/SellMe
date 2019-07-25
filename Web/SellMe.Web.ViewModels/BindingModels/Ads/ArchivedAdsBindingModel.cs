namespace SellMe.Web.ViewModels.BindingModels.Ads
{
    using System.Collections.Generic;
    using SellMe.Web.ViewModels.ViewModels;
    using SellMe.Web.ViewModels.ViewModels.Ads;
    public class ArchivedAdsBindingModel : BaseViewModel
    {
        public IEnumerable<MyArchivedAdsViewModel> Ads { get; set; }
    }
}
