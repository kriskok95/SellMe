namespace SellMe.Web.ViewModels.InputModels.Ads
{
    using SellMe.Web.ViewModels.ViewModels;

    public class CreateAdInputModel : BaseViewModel
    {
        public CreateAdDetailInputModel CreateAdDetailInputModel { get; set; }

        public CreateAdAddressInputModel CreateAdAddressInputModel { get; set; }
    }
}
