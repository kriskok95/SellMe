namespace SellMe.Web.ViewModels.ViewModels.Ads
{
    using SellMe.Web.ViewModels.ViewModels.Subcategories;
    using System.Collections.Generic;

    public class AdsByCategoryViewModel
    {
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public List<AdsByCategorySubcategoryViewModel> AdsByCategorySubcategoryViewModels { get; set; }

        public ICollection<AdViewModel> AdsViewModels { get; set; }
    }
}
