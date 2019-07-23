namespace SellMe.Web.ViewModels.ViewModels.Home
{
    using System.Collections.Generic;
    using SellMe.Web.ViewModels.ViewModels.Categories;
    using SellMe.Web.ViewModels.ViewModels.Ads;

    public class IndexViewModel
    {
        public List<CategoryViewModel> CategoryViewModels { get; set; }

        public List<PromotedAdViewModel> PromotedAdViewModels { get; set; }

        public List<LatestAddedAdViewModel> LatestAddedAdViewModels { get; set; }
    }
}
