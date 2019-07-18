namespace SellMe.Services.Interfaces
{
    using System.Threading.Tasks;
    using SellMe.Data.Models;
    using System.Collections.Generic;

    public interface IAddressService
    {
        ICollection<string> GetAllCountries();

        Task<Address> GetAddressByAdIdAsync(int addressId);
    }
}
