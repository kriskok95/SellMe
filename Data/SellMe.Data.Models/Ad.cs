namespace SellMe.Data.Models
{
    using System;
    using System.Collections.Generic;
    using SellMe.Data.Common;

    public class Ad : BaseDeletableModel<int>
    {
        public Ad()
        {
            this.SellMeUserFavoriteProducts = new HashSet<SellMeUserFavoriteProduct>();
            this.Reviews = new HashSet<Review>();
            this.Images = new HashSet<Image>();
            this.Messages = new HashSet<Message>();
        }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime ActiveFrom { get; set; }

        public DateTime ActiveTo { get; set; }

        public decimal Price { get; set; }

        public int AvailabilityCount { get; set; }

        public int Updates { get; set; }

        public bool IsApproved { get; set; }

        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

        public int SubCategoryId { get; set; }
        public virtual SubCategory SubCategory { get; set; }

        public int ConditionId { get; set; }
        public virtual Condition Condition { get; set; }

        public string SellerId { get; set; }
        public virtual SellMeUser Seller { get; set; }

        public int AddressId { get; set; }
        public virtual Address Address { get; set; }

        public virtual ICollection<PromotionOrder> PromotionOrders { get; set; }

        public virtual ICollection<SellMeUserFavoriteProduct> SellMeUserFavoriteProducts { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }

        public virtual ICollection<Image> Images { get; set; }

        public virtual ICollection<Message> Messages { get; set; }

        public virtual ICollection<AdView> AdViews { get; set; }

        public virtual ICollection<UpdateAd> UpdateAds { get; set; }
    }
}
