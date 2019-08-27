namespace SellMe.Web.ViewModels.ViewModels.Home
{
    using System.Collections.Generic;
    using Ads;
    using Categories;

    public class IndexViewModel : BaseViewModel
    {
        public List<CategoryViewModel> CategoryViewModels { get; set; }

        public List<PromotedAdViewModel> PromotedAdViewModels { get; set; }

        public List<LatestAddedAdViewModel> LatestAddedAdViewModels { get; set; }
    }
}
