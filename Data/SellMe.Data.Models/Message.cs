namespace SellMe.Data.Models
{
    using Common;

    public class Message : BaseDeletableModel<int>
    {
        public string Content { get; set; }

        public bool IsRead { get; set; }

        public string SenderId { get; set; }
        public virtual SellMeUser Sender { get; set; }

        public string RecipientId { get; set; }
        public virtual SellMeUser Recipient { get; set; }

        public int AdId { get; set; }
        public virtual Ad Ad { get; set; }
    }
}
