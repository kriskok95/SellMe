namespace SellMe.Web.ViewModels.BindingModels.Messages
{
    using InputModels.Messages;
    using ViewModels;
    using ViewModels.Messages;

    public class SendMessageBindingModel : BaseViewModel
    {
        public SendMessageViewModel ViewModel { get; set; }

        public SendMessageInputModel InputModel { get; set; }
    }
}
