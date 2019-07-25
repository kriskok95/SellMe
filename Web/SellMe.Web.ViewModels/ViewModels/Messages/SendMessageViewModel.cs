namespace SellMe.Web.ViewModels.ViewModels.Messages
{
    public class SendMessageViewModel : BaseViewModel
    {
        public string RecipientId { get; set; }

        public string SenderId { get; set; }

        public string SellerPhone { get; set; }

        public int AdId { get; set; }

        public string AdTitle { get; set; }

        public decimal AdPrice { get; set; }
    }
}
