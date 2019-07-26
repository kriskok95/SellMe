namespace SellMe.Data.Models
{
    using SellMe.Data.Common;
    using System;

    public class PromotionOrder : BaseDeletableModel<int>
    {
        public int AdId { get; set; }
        public virtual Ad Ad { get; set; }

        public int PromotionId { get; set; }
        public virtual Promotion Promotion { get; set; }

        public decimal Price { get; set; }

        public bool IsActive => DateTime.UtcNow <= this.ActiveTo;

        public DateTime ActiveTo { get; set; }
    }
}
