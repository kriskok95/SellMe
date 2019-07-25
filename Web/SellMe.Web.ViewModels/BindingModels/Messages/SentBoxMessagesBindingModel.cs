namespace SellMe.Web.ViewModels.BindingModels.Messages
{
    using System.Collections.Generic;
    using SellMe.Web.ViewModels.ViewModels.Messages;
    using SellMe.Web.ViewModels.ViewModels;

    public class SentBoxMessagesBindingModel : BaseViewModel
    {
        public IEnumerable<SentBoxMessageViewModel> Messages { get; set; }
    }
}
