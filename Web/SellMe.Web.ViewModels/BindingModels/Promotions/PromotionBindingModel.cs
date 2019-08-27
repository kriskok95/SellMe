namespace SellMe.Web.ViewModels.BindingModels.Promotions
{
    using System.Collections.Generic;
    using InputModels.Promotions;
    using ViewModels;
    using ViewModels.Promotions;

    public class PromotionBindingModel : BaseViewModel
    {
        public int AdId { get; set; }

        public string AdTitle { get; set; }

        public List<PromotionViewModel> PromotionViewModels { get; set; }

        public PromotionInputModel PromotionInputModel { get; set; }
    }
}
