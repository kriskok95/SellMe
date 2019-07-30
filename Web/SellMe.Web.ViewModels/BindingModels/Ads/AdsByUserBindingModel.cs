namespace SellMe.Web.ViewModels.BindingModels.Ads
{
    using System.Collections.Generic;
    using SellMe.Web.ViewModels.ViewModels.Ads;
    using SellMe.Web.ViewModels.ViewModels;

    public class AdsByUserBindingModel : BaseViewModel
    {
        public string UserId { get; set; }

        public string Username { get; set; }

        public IEnumerable<AdViewModel> AdViewModels { get; set; }
    }
}
