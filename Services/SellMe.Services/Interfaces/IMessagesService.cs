namespace SellMe.Services.Interfaces
{
    using SellMe.Web.ViewModels.ViewModels.Messages;
    using SellMe.Web.ViewModels.BindingModels;
    using SellMe.Web.ViewModels.InputModels.Messages;

    public interface IMessagesService
    {
        SendMessageViewModel GetMessageViewModelByAdId(int adId);

        SendMessageBindingModel GetMessageBindingModelByAdId(int adId);

        void CreateMessage(SendMessageInputModel inputModel);
    }
}
