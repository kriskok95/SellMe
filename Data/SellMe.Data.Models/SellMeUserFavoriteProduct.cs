namespace SellMe.Data.Models
{
    using SellMe.Data.Common;

    public class SellMeUserFavoriteProduct : BaseDeletableModel<int>
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public string SellMeUserId { get; set; }
        public SellMeUser SellMeUser { get; set; }
    }
}
