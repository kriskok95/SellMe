namespace SellMe.Web.ViewComponents
{
    using Microsoft.AspNetCore.Mvc;
    using Services.Interfaces;
    using ViewModels.ViewModels.Ads;

    public class CountriesEditDropdownViewComponent : ViewComponent
    {
        private readonly IAddressesService _addressesService;

        public CountriesEditDropdownViewComponent(IAddressesService addressesService)
        {
            _addressesService = addressesService;
        }

        public IViewComponentResult Invoke(string adCountry)
        {
            var countries = _addressesService.GetAllCountries();

            var countriesDropDownViewModel = new CountriesDropDownEditViewModel
            {
                Countries = countries,
                AdCountry = adCountry
            };

            return View(countriesDropDownViewModel);
        }
    }
}
