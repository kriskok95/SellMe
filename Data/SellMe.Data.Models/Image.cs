namespace SellMe.Data.Models
{
    using Common;

    public class Image : BaseDeletableModel<int>
    {
        public string ImageUrl { get; set; }

        public int AdId { get; set; }

        public virtual Ad Ad { get; set; }
    }
}
