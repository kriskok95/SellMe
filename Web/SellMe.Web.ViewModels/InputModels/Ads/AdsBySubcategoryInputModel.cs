namespace SellMe.Web.ViewModels.InputModels.Ads
{
    public class AdsBySubcategoryInputModel
    {
        public int SubcategoryId { get; set; }

        public int CategoryId { get; set; }

        public int? PageNumber { get; set; }
    }
}
