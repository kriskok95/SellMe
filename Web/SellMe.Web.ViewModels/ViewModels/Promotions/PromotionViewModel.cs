namespace SellMe.Web.ViewModels.ViewModels.Promotions
{
    using SellMe.Data.Models;
    using SellMe.Services.Mapping;

    public class PromotionViewModel : BaseViewModel, IMapFrom<Promotion>
    {
        public int Id { get; set; }

        public string Type { get; set; }

        public int Updates { get; set; }

        public int ActiveDays { get; set; }

        public decimal Price { get; set; }
    }
}
