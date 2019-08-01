namespace SellMe.Web.ViewComponents
{
    using Microsoft.AspNetCore.Mvc;
    using SellMe.Services.Interfaces;

    public class CountriesDropdownViewComponent : ViewComponent
    {
        private readonly IAddressesService _addressesService;

        public CountriesDropdownViewComponent(IAddressesService addressesService)
        {
            this._addressesService = addressesService;
        }

        public IViewComponentResult Invoke()
        {
            var countries = this._addressesService.GetAllCountries();
            return this.View(countries);
        }
    }
}
