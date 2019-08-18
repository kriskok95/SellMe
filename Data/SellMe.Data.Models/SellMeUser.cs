﻿namespace SellMe.Data.Models
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Identity;
    using System;
    using SellMe.Data.Common;


    // Add profile data for application users by adding properties to the SellMeUser class
    public class SellMeUser : IdentityUser, IDeletableEntity
    {
        public SellMeUser()
        {
            this.SellMeUserFavoriteProducts = new HashSet<SellMeUserFavoriteProduct>();
            this.SentBox = new HashSet<Message>();
            this.Ads = new HashSet<Ad>();
        }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public virtual ICollection<SellMeUserAddress> SellMeUserAddresses { get; set; }

        public virtual ICollection<SellMeUserFavoriteProduct> SellMeUserFavoriteProducts { get; set; }

        public virtual ICollection<Message> Inbox { get; set; }

        public virtual ICollection<Message> SentBox { get; set; }

        public virtual ICollection<Ad> Ads { get; set; }

        public virtual ICollection<Review> OwnedReviews { get; set; }

        public virtual ICollection<Review> CreatedReviews { get; set; }
    }
}
