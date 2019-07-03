namespace SellMe.Data.Models
{
    public class SellMeUserAddress
    {
        public string SellMeUserId { get; set; }
        public virtual SellMeUser SellMeUser { get; set; }

        public int AddressId { get; set; }
        public virtual Address Address { get; set; }
    }
}
