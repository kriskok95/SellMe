namespace SellMe.Data.Models
{
    public class SellMeUserAddress
    {
        public string SellMeUserId { get; set; }
        public SellMeUser SellMeUser { get; set; }

        public int AddressId { get; set; }
        public Address Address { get; set; }
    }
}
