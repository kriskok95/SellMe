namespace SellMe.Data.Models
{
    using SellMe.Data.Common;

    public class SellMeUserFavoriteProduct : BaseDeletableModel<int>
    {
        public int AdId { get; set; }
        public Ad Ad { get; set; }

        public string SellMeUserId { get; set; }
        public SellMeUser SellMeUser { get; set; }
    }
}
