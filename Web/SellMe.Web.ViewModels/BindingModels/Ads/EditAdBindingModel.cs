namespace SellMe.Web.ViewModels.BindingModels.Ads
{
    using SellMe.Web.ViewModels.ViewModels.Ads;
    using SellMe.Web.ViewModels.InputModels.Ads;
    using SellMe.Web.ViewModels.ViewModels;

    public class EditAdBindingModel : BaseViewModel
    {
        public EditAdViewModel EditAdViewModel { get; set; }

        public EditAdInputModel EditAdInputModel { get; set; }
    }
}
