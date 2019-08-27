namespace SellMe.Data.Models
{
    using Common;

    public class SellMeUserFavoriteProduct : BaseDeletableModel<int>
    {
        public int AdId { get; set; }
        public virtual Ad Ad { get; set; }

        public string SellMeUserId { get; set; }
        public virtual SellMeUser SellMeUser { get; set; }
    }
}
