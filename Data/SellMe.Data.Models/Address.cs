using System.Collections.Generic;

namespace SellMe.Data.Models
{
    using SellMe.Data.Common;

    public class Address : BaseDeletableModel<int>
    {
        public string Country { get; set; }

        public string City { get; set; }

        public string Street { get; set; }

        public string ZipCode { get; set; }

        public ICollection<SellMeUserAddress> SellMeUserAddresses { get; set; }

        public ICollection<Ad> Ads { get; set; }
    }
}
