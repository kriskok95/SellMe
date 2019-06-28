namespace SellMe.Web.ViewModels.InputModels.Products
{
    using SellMe.Data.Models;
    using SellMe.Services.Mapping;
    using System.ComponentModel.DataAnnotations;

    public class CreateAdAddressInputModel : IMapTo<Address>
    {
        [Required]
        public string Country { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string District { get; set; }

        [Required]
        public string  Street { get; set; }

        [Required]
        [DataType(DataType.PostalCode)]
        public int ZipCode { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }
    }
}
