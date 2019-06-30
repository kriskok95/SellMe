namespace SellMe.Web.ViewModels.ViewModels.Ads
{
    using System.Collections.Generic;
    using SellMe.Web.ViewModels.ViewModels.Categories;

    public class AdsByCategoryViewModel
    {
        public string CategoryName { get; set; }

        public ICollection<AdViewModel> AdsViewModels { get; set; }

        public ICollection<CategoryViewModel> Categories { get; set; }
    }
}
