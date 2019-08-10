namespace SellMe.Services.Interfaces
{
    using System.Threading.Tasks;
    using SellMe.Web.ViewModels.ViewModels.Messages;
    using SellMe.Web.ViewModels.InputModels.Messages;
    using System.Collections.Generic;
    using SellMe.Web.ViewModels.BindingModels.Messages;

    public interface IMessagesService
    {
        Task<SendMessageViewModel> GetMessageViewModelByAdIdAsync(int adId);

        Task<SendMessageBindingModel> GetMessageBindingModelByAdIdAsync(int adId);

        Task<MessageDetailsViewModel> CreateMessageAsync(SendMessageInputModel inputModel);

        Task<ICollection<MessageDetailsViewModel>> GetMessageDetailsViewModelsAsync(int adId, string senderId, string sellerId);

        Task<int> GetUnreadMessagesCountAsync(string userId);

        Task<InboxMessagesBindingModel> GetInboxMessagesBindingModelAsync();

        Task<SentBoxMessagesBindingModel>GetSentBoxMessagesBindingModelAsync();
    }
}
