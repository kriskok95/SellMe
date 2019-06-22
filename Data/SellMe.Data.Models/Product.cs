﻿namespace SellMe.Data.Models
{
    using SellMe.Data.Models.Enums;
    using System.Collections.Generic;
    using SellMe.Data.Common;


    public class Product : BaseDeletableModel<int>
    {
        public Product()
        {
            this.SellMeUserFavoriteProducts = new HashSet<SellMeUserFavoriteProduct>();
            this.Reviews = new HashSet<Review>();
            this.Images = new HashSet<Image>();
            this.Messages = new HashSet<Message>();
        }

        public string Title { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public Condition Condition { get; set; }

        public int AvailabilityCount { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public int SubCategoryId { get; set; }
        public SubCategory SubCategory { get; set; }

        public ICollection<SellMeUserFavoriteProduct> SellMeUserFavoriteProducts { get; set; }

        public ICollection<Review> Reviews { get; set; }

        public ICollection<Image> Images { get; set; }

        public ICollection<Message> Messages { get; set; }
    }
}
