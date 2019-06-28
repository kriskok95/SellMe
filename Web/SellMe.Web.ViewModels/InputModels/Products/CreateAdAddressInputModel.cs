namespace SellMe.Web.ViewModels.InputModels.Products
{
    using SellMe.Data.Models;
    using SellMe.Services.Mapping;

    public class CreateAdAddressInputModel : IMapTo<Address>
    {
        public string Country { get; set; }

        public string City { get; set; }

        public string District { get; set; }

        public string  Street { get; set; }

        public int ZipCode { get; set; }

        public string PhoneNumber { get; set; }

        public string EmailAddress { get; set; }
    }
}
