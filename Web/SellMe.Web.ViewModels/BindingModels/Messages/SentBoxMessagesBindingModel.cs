namespace SellMe.Web.ViewModels.BindingModels.Messages
{
    using System.Collections.Generic;
    using ViewModels;
    using ViewModels.Messages;

    public class SentBoxMessagesBindingModel : BaseViewModel
    {
        public IEnumerable<SentBoxMessageViewModel> Messages { get; set; }
    }
}
