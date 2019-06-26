namespace SellMe.Data.Models
{
    using SellMe.Data.Common;

    public class Review : BaseDeletableModel<int>
    {
        public int Rating { get; set; }

        public string Comment { get; set; }

        public int AdId { get; set; }
        public Ad Ad { get; set; }
    }
}
