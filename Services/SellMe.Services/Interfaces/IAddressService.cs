using SellMe.Web.ViewModels.InputModels.Ads;

namespace SellMe.Services.Interfaces
{
    using SellMe.Data.Models;
    using System.Collections.Generic;

    public interface IAddressService
    {
        ICollection<string> GetAllCountries();

        Address CreateAddress(CreateAdAddressInputModel inputModelCreateAdAddressInputModel);
    }
}
