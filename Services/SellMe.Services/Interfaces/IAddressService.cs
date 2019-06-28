namespace SellMe.Services.Interfaces
{
    using SellMe.Data.Models;
    using SellMe.Web.ViewModels.InputModels.Products;
    using System.Collections.Generic;

    public interface IAddressService
    {
        ICollection<string> GetAllCountries();

        Address CreateAddress(CreateAdAddressInputModel inputModelCreateAdAddressInputModel);
    }
}
