namespace SellMe.Web.ViewModels.BindingModels.Messages
{
    using System.Collections.Generic;
    using SellMe.Web.ViewModels.InputModels.Messages;
    using SellMe.Web.ViewModels.ViewModels.Messages;

    public class MessageDetailsBindingModel
    {
        public string SenderId { get; set; }

        public string SellerId { get; set; }

        public int AdId { get; set; }

        public string AdTitle { get; set; }

        public ICollection<MessageDetailsViewModel> ViewModels { get; set; }

        public MessageDetailsInputModel InputModel { get; set; }
    }
}
