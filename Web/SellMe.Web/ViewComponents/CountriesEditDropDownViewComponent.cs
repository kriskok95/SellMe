using SellMe.Services.Interfaces;
using SellMe.Web.ViewModels.ViewModels.Ads;

namespace SellMe.Web.ViewComponents
{
    using Microsoft.AspNetCore.Mvc;

    public class CountriesEditDropdownViewComponent : ViewComponent
    {
        private readonly IAddressesService _addressesService;

        public CountriesEditDropdownViewComponent(IAddressesService addressesService)
        {
            this._addressesService = addressesService;
        }

        public IViewComponentResult Invoke(string adCountry)
        {
            var countries = this._addressesService.GetAllCountries();

            var countriesDropDownViewModel = new CountriesDropDownEditViewModel
            {
                Countries = countries,
                AdCountry = adCountry
            };

            return this.View(countriesDropDownViewModel);
        }
    }
}
