namespace SellMe.Web.ViewModels.BindingModels.Ads
{
    using InputModels.Ads;
    using ViewModels;
    using ViewModels.Ads;

    public class EditAdBindingModel : BaseViewModel
    {
        public EditAdViewModel EditAdViewModel { get; set; }

        public EditAdInputModel EditAdInputModel { get; set; }
    }
}
