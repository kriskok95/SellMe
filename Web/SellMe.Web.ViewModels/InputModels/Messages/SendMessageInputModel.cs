namespace SellMe.Web.ViewModels.InputModels.Messages
{
    public class SendMessageInputModel
    {
        public string Content { get; set; }

        public string SenderId { get; set; }

        public string RecipientId { get; set; }

        public int AdId { get; set; }
    }
}
