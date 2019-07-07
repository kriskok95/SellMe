namespace SellMe.Web.ViewModels.ViewModels.Ads
{
    using SellMe.Web.ViewModels.ViewModels.Categories;
    using System.Collections.Generic;

    public class AdsAllViewModel 
    {
        public ICollection<AdViewModel> AdsViewModels { get; set; }
    }
}
