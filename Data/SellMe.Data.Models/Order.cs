using SellMe.Data.Common;

namespace SellMe.Data.Models
{
    public class Order : BaseDeletableModel<int>
    {
        public int AdId { get; set; }
        public Ad Ad { get; set; }

        public string UserId { get; set; }
        public SellMeUser User { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

    }
}
