namespace SellMe.Web.ViewModels.InputModels.Ads
{
    using ViewModels;

    public class CreateAdInputModel : BaseViewModel
    {
        public CreateAdDetailInputModel CreateAdDetailInputModel { get; set; }

        public CreateAdAddressInputModel CreateAdAddressInputModel { get; set; }
    }
}
