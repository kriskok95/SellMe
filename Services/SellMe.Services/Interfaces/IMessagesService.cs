namespace SellMe.Services.Interfaces
{
    using SellMe.Web.ViewModels.ViewModels.Messages;
    using SellMe.Web.ViewModels.InputModels.Messages;
    using System.Collections.Generic;
    using SellMe.Web.ViewModels.BindingModels.Messages;

    public interface IMessagesService
    {
        SendMessageViewModel GetMessageViewModelByAdId(int adId);

        SendMessageBindingModel GetMessageBindingModelByAdId(int adId);

        void CreateMessage(SendMessageInputModel inputModel);

        ICollection<InboxMessageViewModel> GetInboxViewModelsByCurrentUser();

        ICollection<SentBoxMessageViewModel> GetSentBoxViewModelByCurrentUser();

        ICollection<MessageDetailsViewModel> GetMessageDetailsViewModels(int adId, string senderId, string sellerId);
    }
}
