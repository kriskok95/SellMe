namespace SellMe.Data.Models
{
    using SellMe.Data.Common;

    public class SellMeUserFavoriteProduct : BaseModel<int>
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public string SellMeUserId { get; set; }
        public SellMeUser SellMeUser { get; set; }
    }
}
