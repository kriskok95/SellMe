using SellMe.Web.ViewModels.InputModels.Ads;

namespace SellMe.Services
{
    using System.Collections.Generic;
    using SellMe.Services.Interfaces;
    using System.Globalization;
    using AutoMapper;
    using SellMe.Data.Models;
    using SellMe.Data;

    public class AddressService : IAddressService
    {
        private readonly SellMeDbContext context;

        public AddressService(SellMeDbContext context)
        {
            this.context = context;
        }

        public ICollection<string> GetAllCountries()
        {
            //Creating list
            var CultureList = new List<string>();

            //Getting the specific CultureInfo from CultureInfo Class 

            var getCultureInfo = CultureInfo.GetCultures(CultureTypes.SpecificCultures);

            foreach (CultureInfo getCulture in getCultureInfo)
            {
                //Creating the object of RegionInfo class
                RegionInfo getRegionInfo = new RegionInfo(getCulture.LCID);

                //Adding each country name into the arrayList
                if (!(CultureList.Contains(getRegionInfo.EnglishName)))
                {
                    CultureList.Add(getRegionInfo.EnglishName);
                }
            }

            //Sorting countries
            CultureList.Sort();

            return CultureList;
        }

        public Address CreateAddress(CreateAdAddressInputModel inputModelCreateAdAddressInputModel)
        {
            var address = Mapper.Map<Address>(inputModelCreateAdAddressInputModel);
            this.context.Addresses.Add(address);
            this.context.SaveChanges();

            return address;
        }
    }
}
