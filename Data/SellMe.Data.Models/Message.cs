namespace SellMe.Data.Models
{
    using SellMe.Data.Common;

    public class Message : BaseModel<int>
    {
        public string Content { get; set; }

        public string SenderId { get; set; }
        public SellMeUser Sender { get; set; }

        public string RecipientId { get; set; }
        public SellMeUser Recipient { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
