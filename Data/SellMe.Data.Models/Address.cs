using SellMe.Data.Common;

namespace SellMe.Data.Models
{
    public class Address : BaseModel<int>
    {
        public string Country { get; set; }

        public string City { get; set; }

        public string Street { get; set; }

        public string ZipCode { get; set; }

        public SellMeUser SellMeUser { get; set; }
    }
}
