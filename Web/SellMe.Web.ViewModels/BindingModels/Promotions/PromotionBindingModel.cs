namespace SellMe.Web.ViewModels.BindingModels.Promotions
{
    using SellMe.Web.ViewModels.ViewModels.Promotions;
    using SellMe.Web.ViewModels.InputModels.Promotions;
    using SellMe.Web.ViewModels.ViewModels;

    public class PromotionBindingModel : BaseViewModel
    {
        public PromotionViewModel PromotionViewModel { get; set; }

        public PromotionInputModel PromotionInputModel { get; set; }
    }
}
