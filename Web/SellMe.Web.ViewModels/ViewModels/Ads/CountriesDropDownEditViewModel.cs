namespace SellMe.Web.ViewModels.ViewModels.Ads
{
    using System.Collections.Generic;

    public class CountriesDropDownEditViewModel
    {
        public IEnumerable<string> Countries { get; set; }

        public string AdCountry { get; set; }
    }
}
