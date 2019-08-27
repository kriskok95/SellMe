namespace SellMe.Web.ViewModels.ViewModels.Ads
{
    using System.Collections.Generic;
    using Services.Paging;
    using Subcategories;

    public class AdsBySubcategoryViewModel : BaseViewModel
    {
        public int CategoryId { get; set; }

        public int SubcategoryId { get; set; }

        public PaginatedList<AdViewModel> AdsBySubcategoryViewModels { get; set; }

        public List<AdsByCategorySubcategoryViewModel> AdsByCategorySubcategoryViewModels { get; set; }
    }
}
