namespace SellMe.Data.Models
{
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
            this.Orders = new HashSet<Order>();
        }

        public string Title { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public int AvailabilityCount { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public int SubCategoryId { get; set; }
        public SubCategory SubCategory { get; set; }

        public int ConditionId { get; set; }
        public Condition Condition { get; set; }

        public string SellerId { get; set; }
        public SellMeUser Seller { get; set; }

        public int? AddressId { get; set; }
        public Address Address { get; set; }

        public ICollection<SellMeUserFavoriteProduct> SellMeUserFavoriteProducts { get; set; }

        public ICollection<Review> Reviews { get; set; }

        public ICollection<Image> Images { get; set; }

        public ICollection<Message> Messages { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}
