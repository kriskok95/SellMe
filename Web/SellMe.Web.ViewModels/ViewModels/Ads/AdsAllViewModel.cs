using SellMe.Services.Paging;

namespace SellMe.Web.ViewModels.ViewModels.Ads
{
    using SellMe.Web.ViewModels.ViewModels.Categories;
    using System.Collections.Generic;

    public class AdsAllViewModel : BaseViewModel
    {
        public PaginatedList<AdViewModel> AdsViewModels { get; set; }
    }
}
