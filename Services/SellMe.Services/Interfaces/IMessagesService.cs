namespace SellMe.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Web.ViewModels.BindingModels.Messages;
    using Web.ViewModels.ViewModels.Messages;

    public interface IMessagesService
    {
        Task<SendMessageViewModel> GetMessageViewModelByAdIdAsync(int adId);

        Task<SendMessageBindingModel> GetMessageBindingModelByAdIdAsync(int adId);

        Task<MessageDetailsViewModel> CreateMessageAsync(string senderId, string recipientId, int adId, string content);

        Task<ICollection<MessageDetailsViewModel>> GetMessageDetailsViewModelsAsync(int adId, string senderId, string recipientId);

        Task<int> GetUnreadMessagesCountAsync(string userId);

        Task<IEnumerable<InboxMessageViewModel>> GetInboxMessagesViewModelsAsync();

        Task<SentBoxMessagesBindingModel>GetSentBoxMessagesBindingModelAsync();
    }
}
