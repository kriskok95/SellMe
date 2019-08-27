namespace SellMe.Web.ViewComponents
{
    using Microsoft.AspNetCore.Mvc;
    using Services.Interfaces;

    public class CountriesDropdownViewComponent : ViewComponent
    {
        private readonly IAddressesService _addressesService;

        public CountriesDropdownViewComponent(IAddressesService addressesService)
        {
            _addressesService = addressesService;
        }

        public IViewComponentResult Invoke()
        {
            var countries = _addressesService.GetAllCountries();
            return View(countries);
        }
    }
}
