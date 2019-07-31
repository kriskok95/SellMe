namespace SellMe.Web.ViewModels.ViewModels.Ads
{
    using SellMe.Web.ViewModels.ViewModels.Subcategories;
    using SellMe.Services.Paging;
    using System.Collections.Generic;

    public class AdsByCategoryViewModel : BaseViewModel
    {
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public List<AdsByCategorySubcategoryViewModel> AdsByCategorySubcategoryViewModels { get; set; }

        public PaginatedList<AdViewModel> AdsViewModels { get; set; }
    }
}
