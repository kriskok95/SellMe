namespace SellMe.Data.Models
{
    using System;
    using SellMe.Data.Common;
    using System.Collections.Generic;

    public class Promotion : BaseModel<int>
    {
        public string Type { get; set; }

        public decimal Price { get; set; }

        public int ActiveDays { get; set; }

        public int Updates { get; set; }

        public virtual ICollection<PromotionOrder> PromotionOrders { get; set; }
    }
}
