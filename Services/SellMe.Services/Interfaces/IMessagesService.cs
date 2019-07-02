namespace SellMe.Services.Interfaces
{
    using SellMe.Web.ViewModels.ViewModels.Messages;

    public interface IMessagesService
    {
        SendMessageViewModel GetMessageViewModelByAdId(int adId);
    }
}
