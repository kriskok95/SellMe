namespace SellMe.Web.ViewModels.ViewModels.Ads
{
    using System.Collections.Generic;
    using SellMe.Web.ViewModels.ViewModels.Subcategories;

    public class AdsBySubcategoryViewModel
    {
        public List<AdDetailsViewModel> AdsBySubcategoryViewModels { get; set; }

        public List<AdsByCategorySubcategoryViewModel> AdsByCategorySubcategoryViewModels { get; set; }
    }
}
