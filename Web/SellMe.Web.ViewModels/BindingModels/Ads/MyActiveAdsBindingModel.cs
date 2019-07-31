using SellMe.Services.Paging;

namespace SellMe.Web.ViewModels.BindingModels.Ads
{
    using SellMe.Web.ViewModels.ViewModels;
    using System.Collections.Generic;
    using SellMe.Web.ViewModels.ViewModels.Ads;


    public class MyActiveAdsBindingModel : BaseViewModel
    {
        public PaginatedList<MyActiveAdsViewModel> Ads { get; set; }
    }
}
