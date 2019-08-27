namespace SellMe.Data.Models
{
    using System.Collections.Generic;
    using Common;

    public class Promotion : BaseModel<int>
    {
        public string Type { get; set; }

        public decimal Price { get; set; }

        public int ActiveDays { get; set; }

        public int Updates { get; set; }

        public virtual ICollection<PromotionOrder> PromotionOrders { get; set; }
    }
}
