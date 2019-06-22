using System;
using SellMe.Data.Common;

namespace SellMe.Data.Models
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Identity;

    // Add profile data for application users by adding properties to the SellMeUser class
    public class SellMeUser : IdentityUser, IDeletableEntity
    {
        public SellMeUser()
        {
            this.SellMeUserFavoriteProducts = new HashSet<SellMeUserFavoriteProduct>();
            this.SentBox = new HashSet<Message>();
        }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public int? AddressId { get; set; }
        public Address Address { get; set; }

        public ICollection<SellMeUserFavoriteProduct> SellMeUserFavoriteProducts { get; set; }

        public ICollection<Message> Inbox { get; set; }

        public ICollection<Message> SentBox { get; set; }
    }
}
