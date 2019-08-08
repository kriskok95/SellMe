namespace SellMe.Web.ViewModels.InputModels.Ads
{
    public class EditAdInputModel
    {
        public int AdId { get; set; }

        public EditAdDetailsInputModel EditAdDetailsInputModel { get; set; }

        public EditAdAddressInputModel EditAdAddressInputModel { get; set; }
    }
}
