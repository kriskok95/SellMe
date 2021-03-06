﻿namespace SellMe.Services
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using Data;
    using Data.Models;
    using Interfaces;
    using Microsoft.EntityFrameworkCore;

    public class AddressesService : IAddressesService
    {
        public const string InvalidAddressIdErrorMessage = "Address with the given ID doesn't exist!";
        private readonly SellMeDbContext context;

        public AddressesService(SellMeDbContext context)
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

        public async Task<Address> GetAddressByIdAsync(int addressId)
        {
            if (!context.Addresses.Any(x => x.Id == addressId))
            {
                throw new ArgumentException(InvalidAddressIdErrorMessage);
            }
            var address = await context
                .Addresses
                .FirstOrDefaultAsync(x => x.Id == addressId);

            return address;
        }
    }
}
