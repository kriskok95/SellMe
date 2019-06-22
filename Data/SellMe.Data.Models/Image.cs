namespace SellMe.Data.Models
{
    using SellMe.Data.Common;

    public class Image : BaseDeletableModel<int>
    {
        public string ImageUrl { get; set; }

        public int ProductId { get; set; }

        public virtual Product Product { get; set; }
    }
}
