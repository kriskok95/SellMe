namespace SellMe.Web.ViewModels.BindingModels.Promotions
{
    using SellMe.Web.ViewModels.ViewModels.Promotions;
    using SellMe.Web.ViewModels.InputModels.Promotions;
    using SellMe.Web.ViewModels.ViewModels;
    using System.Collections.Generic;

    public class PromotionBindingModel : BaseViewModel
    {
        public int AdId { get; set; }

        public string AdTitle { get; set; }

        public List<PromotionViewModel> PromotionViewModels { get; set; }

        public PromotionInputModel PromotionInputModel { get; set; }
    }
}
