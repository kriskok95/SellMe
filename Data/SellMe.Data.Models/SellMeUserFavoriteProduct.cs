namespace SellMe.Data.Models
{
    public class SellMeUserFavoriteProduct
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public string SellMeUserId { get; set; }
        public SellMeUser SellMeUser { get; set; }
    }
}
