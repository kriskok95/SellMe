namespace SellMe.Data.Models
{
    using System;
    using Common;

    public class PromotionOrder : BaseDeletableModel<int>
    {
        public int AdId { get; set; }
        public virtual Ad Ad { get; set; }

        public int PromotionId { get; set; }
        public virtual Promotion Promotion { get; set; }

        public decimal Price { get; set; }

        public bool IsActive => DateTime.UtcNow <= ActiveTo;

        public DateTime ActiveTo { get; set; }
    }
}
