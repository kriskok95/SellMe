namespace SellMe.Web.ViewModels.ViewModels.Ads
{
    using Services.Paging;

    public class AdsBySearchViewModel
    {
        public string  Search { get; set; }

        public PaginatedList<AdViewModel> Ads { get; set; }
    }
}
