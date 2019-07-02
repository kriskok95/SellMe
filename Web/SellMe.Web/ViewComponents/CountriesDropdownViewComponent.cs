namespace SellMe.Web.ViewComponents
{
    using Microsoft.AspNetCore.Mvc;
    using SellMe.Services.Interfaces;

    public class CountriesDropdownViewComponent : ViewComponent
    {
        private readonly IAddressService addressService;

        public CountriesDropdownViewComponent(IAddressService addressService)
        {
            this.addressService = addressService;
        }

        public IViewComponentResult Invoke()
        {
            var countries = this.addressService.GetAllCountries();
            return this.View(countries);
        }
    }
}
