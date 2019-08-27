namespace SellMe.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Data.Models;

    public interface IAddressesService
    {
        ICollection<string> GetAllCountries();

        Task<Address> GetAddressByIdAsync(int addressId);
    }
}
