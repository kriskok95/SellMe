namespace SellMe.Web.ViewModels.BindingModels.Ads
{
    using SellMe.Web.ViewModels.ViewModels;
    using System.Collections.Generic;
    using SellMe.Web.ViewModels.ViewModels.Ads;


    public class MyActiveAdsBindingModel : BaseViewModel
    {
        public IEnumerable<MyActiveAdsViewModel> Ads { get; set; }
    }
}
