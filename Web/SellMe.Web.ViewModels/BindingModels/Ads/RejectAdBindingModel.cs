namespace SellMe.Web.ViewModels.BindingModels.Ads
{
    using SellMe.Web.ViewModels.ViewModels;
    using SellMe.Web.ViewModels.ViewModels.Ads;
    using SellMe.Web.ViewModels.InputModels.Ads;

    public class RejectAdBindingModel : BaseViewModel
    {
        public RejectAdViewModel ViewModel { get; set; }

        public RejectAdInputModel InputModel { get; set; }
    }
}
