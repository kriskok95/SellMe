namespace SellMe.Data.Models
{
    using System.Collections.Generic;
    using Common;

    public class Address : BaseDeletableModel<int>
    {
        public string Country { get; set; }

        public string City { get; set; }

        public string Street { get; set; }

        public string District { get; set; }

        public int ZipCode { get; set; }

        public string PhoneNumber { get; set; }

        public string EmailAddress { get; set; }

        public virtual ICollection<SellMeUserAddress> SellMeUserAddresses { get; set; }

        public virtual ICollection<Ad> Ads { get; set; }
    }
}
