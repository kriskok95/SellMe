namespace SellMe.Data.Models
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
            this.Orders = new HashSet<Order>();
            this.Ads = new HashSet<Ad>();
        }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public ICollection<SellMeUserAddress> SellMeUserAddresses { get; set; }

        public ICollection<SellMeUserFavoriteProduct> SellMeUserFavoriteProducts { get; set; }

        public ICollection<Message> Inbox { get; set; }

        public ICollection<Message> SentBox { get; set; }

        public ICollection<Ad> Ads { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}
