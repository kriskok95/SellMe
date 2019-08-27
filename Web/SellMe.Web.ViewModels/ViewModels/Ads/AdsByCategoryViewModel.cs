namespace SellMe.Web.ViewModels.ViewModels.Ads
{
    using System.Collections.Generic;
    using Services.Paging;
    using Subcategories;

    public class AdsByCategoryViewModel : BaseViewModel
    {
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public List<AdsByCategorySubcategoryViewModel> AdsByCategorySubcategoryViewModels { get; set; }

        public PaginatedList<AdViewModel> AdsViewModels { get; set; }
    }
}
