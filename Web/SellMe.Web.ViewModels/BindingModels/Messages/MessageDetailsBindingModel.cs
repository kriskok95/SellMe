using System.Reflection.Metadata.Ecma335;

namespace SellMe.Web.ViewModels.BindingModels.Messages
{
    public class MessageDetailsBindingModel
    {
        public string SenderId { get; set; }

        public string RecipientId { get; set; }

        public int AdId { get; set; }
    }
}
