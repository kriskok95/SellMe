namespace SellMe.Web.ViewModels.BindingModels.Ads
{
    using InputModels.Ads;
    using ViewModels;
    using ViewModels.Ads;

    public class RejectAdBindingModel : BaseViewModel
    {
        public RejectAdViewModel ViewModel { get; set; }

        public RejectAdInputModel InputModel { get; set; }
    }
}
