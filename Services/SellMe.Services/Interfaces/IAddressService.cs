namespace SellMe.Services.Interfaces
{
    using SellMe.Data.Models;
    using System.Collections.Generic;
    using SellMe.Web.ViewModels.InputModels.Ads;

    public interface IAddressService
    {
        ICollection<string> GetAllCountries();

        Address CreateAddress(CreateAdAddressInputModel inputModelCreateAdAddressInputModel);

        Address GetAddressByAdId(int addressId);
    }
}
