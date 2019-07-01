namespace SellMe.Services.Interfaces
{
    using SellMe.Data.Models;
    using System.Collections.Generic;

    public interface IAddressService
    {
        ICollection<string> GetAllCountries();

        Address GetAddressByAdId(int addressId);
    }
}
