namespace SellMe.Web.ViewModels.ViewModels.Addresses
{
    using SellMe.Data.Models;
    using SellMe.Services.Mapping;

    public class AddressViewModel
    {
        public string Country { get; set; }

        public string City { get; set; }

        public string Street { get; set; }

        public string District { get; set; }
    }
}
