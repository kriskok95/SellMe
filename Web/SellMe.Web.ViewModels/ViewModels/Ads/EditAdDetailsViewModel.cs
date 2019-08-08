namespace SellMe.Web.ViewModels.ViewModels.Ads
{
    using System.Collections.Generic;

    public class EditAdDetailsViewModel : BaseViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public string CategoryName { get; set; }

        public string SubcategoryName { get; set; }

        public string ConditionName { get; set; }

        public int Availability { get; set; }

        public List<string> Images { get; set; }
    }
}
