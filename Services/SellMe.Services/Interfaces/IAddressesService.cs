namespace SellMe.Services.Interfaces
{
    using System.Threading.Tasks;
    using SellMe.Data.Models;
    using System.Collections.Generic;

    public interface IAddressesService
    {
        ICollection<string> GetAllCountries();

        Task<Address> GetAddressByIdAsync(int addressId);
    }
}
