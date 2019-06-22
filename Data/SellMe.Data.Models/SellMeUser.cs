namespace SellMe.Data.Models
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Identity;

    // Add profile data for application users by adding properties to the SellMeUser class
    public class SellMeUser : IdentityUser
    {
        public SellMeUser()
        {
            this.SellMeUserFavoriteProducts = new HashSet<SellMeUserFavoriteProduct>();
            this.SentBox = new HashSet<Message>();
        } 

        public int? AddressId { get; set; }
        public Address Address { get; set; }

        public ICollection<SellMeUserFavoriteProduct> SellMeUserFavoriteProducts { get; set; }

        public ICollection<Message> Inbox { get; set; }

        public ICollection<Message> SentBox { get; set; }
    }
}
