namespace SellMe.Web.ViewModels.ViewModels.Ads
{
    using System.Collections.Generic;
    using SellMe.Web.ViewModels.ViewModels.Subcategories;

    public class AdsBySubcategoryViewModel
    {
        public int CategoryId { get; set; }

        public int SubcategoryId { get; set; }

        public List<AdViewModel> AdsBySubcategoryViewModels { get; set; }

        public List<AdsByCategorySubcategoryViewModel> AdsByCategorySubcategoryViewModels { get; set; }
    }
}
