namespace SellMe.Web.ViewModels.ViewModels.Ads
{
    using System;
    using System.Collections.Generic;
    using SellMe.Web.ViewModels.ViewModels.Addresses;

    public class AdDetailsViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public string Phone { get; set; }

        public string CategoryName { get; set; }

        public string SubcategoryName { get; set; }

        //TODO: Change property name to type
        public string ConditionName { get; set; }

        public DateTime CreatedOn { get; set; }

        public int Views { get; set; }

        public AddressViewModel AddressViewModel { get; set; }

        public ICollection<string> Images { get; set; }
    }
}
