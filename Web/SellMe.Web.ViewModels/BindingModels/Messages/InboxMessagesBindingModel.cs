namespace SellMe.Web.ViewModels.BindingModels.Messages
{
    using System.Collections.Generic;
    using SellMe.Web.ViewModels.ViewModels.Messages;
    using SellMe.Web.ViewModels.ViewModels;

    public class InboxMessagesBindingModel : BaseViewModel
    {
        public IEnumerable<InboxMessageViewModel> Messages { get; set; }
    }
}
