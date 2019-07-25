namespace SellMe.Services
{
    using AutoMapper;
    using System.Threading.Tasks;
    using SellMe.Data;
    using Microsoft.EntityFrameworkCore;
    using SellMe.Data.Models;
    using SellMe.Services.Interfaces;
    using SellMe.Web.ViewModels.ViewModels.Messages;
    using SellMe.Web.ViewModels.InputModels.Messages;
    using System.Collections.Generic;
    using System.Linq;
    using SellMe.Services.Mapping;
    using SellMe.Web.ViewModels.BindingModels.Messages;

    public class MessagesService : IMessagesService
    {
        private readonly IAdsService adsService;
        private readonly IMapper mapper;
        private readonly IUsersService usersService;
        private readonly SellMeDbContext context;

        public MessagesService(IAdsService adsService, IMapper mapper, IUsersService usersService, SellMeDbContext context)
        {
            this.adsService = adsService;
            this.mapper = mapper;
            this.usersService = usersService;
            this.context = context;
        }

        public async Task<SendMessageViewModel> GetMessageViewModelByAdIdAsync(int adId)
        {
            var ad = await this.adsService.GetAdByIdAsync(adId);

            if (ad == null)
            {
                return null;
            }

            var sendMessageViewModel = mapper.Map<SendMessageViewModel>(ad);
            sendMessageViewModel.SenderId = this.usersService.GetCurrentUserId();

            return sendMessageViewModel;
        }

        public async Task<SendMessageBindingModel> GetMessageBindingModelByAdIdAsync(int adId)
        {
            var sendMessageViewModel = await this.GetMessageViewModelByAdIdAsync(adId);
            var sendMessageBindingModel = new SendMessageBindingModel
            {
                ViewModel = sendMessageViewModel
            };

            return sendMessageBindingModel;
        }

        public async Task CreateMessageAsync(SendMessageInputModel inputModel)
        {
            var message = this.mapper.Map<Message>(inputModel);

            await this.context.Messages.AddAsync(message);
            await this.context.SaveChangesAsync();
        }

        private async Task<ICollection<InboxMessageViewModel>> GetInboxViewModelsByCurrentUserAsync()
        {
            var currentUserId = this.usersService.GetCurrentUserId();

            var inboxMessagesFromDb = this.GetInboxMessagesByUserId(currentUserId);
            var inboxMessageViewModels = await inboxMessagesFromDb
                .To<InboxMessageViewModel>()
                .ToListAsync();

            return inboxMessageViewModels;
        }

        private async Task<ICollection<SentBoxMessageViewModel>> GetSentBoxViewModelByCurrentUserAsync()
        {
            var currentUserId = this.usersService.GetCurrentUserId();

            var sentBoxMessagesFromDb = this.GetSentBoxMessagesByUserId(currentUserId);
            var sentBoxMessageViewModels = await sentBoxMessagesFromDb
                .To<SentBoxMessageViewModel>()
                .ToListAsync();

            return sentBoxMessageViewModels;
        }

        public async Task<ICollection<MessageDetailsViewModel>> GetMessageDetailsViewModelsAsync(int adId, string senderId, string recipientId)
        {
            var messagesFromFb = this.GetMessagesDetailsByAd(adId, senderId, recipientId);

            var messageDetailsViewModels = await messagesFromFb
                .To<MessageDetailsViewModel>()
                .ToListAsync();

            return messageDetailsViewModels;
        }

        public async Task<InboxMessagesBindingModel> GetInboxMessagesBindingModelAsync()
        {
            var inboxMessageViewModels = await this.GetInboxViewModelsByCurrentUserAsync();

            var inboxMessagesBindingModel = new InboxMessagesBindingModel
            {
                Messages = inboxMessageViewModels
            };

            return inboxMessagesBindingModel;
        }

        public async Task<SentBoxMessagesBindingModel> GetSentBoxMessagesBindingModelAsync()
        {
            var sentBoxMessageViewModel = await this.GetSentBoxViewModelByCurrentUserAsync();

            var sentBoxMessagesBindingModel = new SentBoxMessagesBindingModel
            {
                Messages = sentBoxMessageViewModel
            };

            return sentBoxMessagesBindingModel;
        }

        private IQueryable<Message> GetMessagesDetailsByAd(int adId, string senderId, string sellerId)
        {
            //TODO: maybe that can cause some bugs
            var messagesFromDb = this.context.Messages
                .Where(x => x.AdId == adId && (x.SenderId == senderId || x.SenderId == sellerId) && (x.RecipientId == sellerId || x.RecipientId == senderId))
                .OrderBy(date => date.CreatedOn);

            return messagesFromDb;
        }

        private IQueryable<Message> GetSentBoxMessagesByUserId(string currentUserId)
        {
            var sentBoxMessages = this.context
                .Ads
                .Where(x => x.Messages.Any(y => y.SenderId == currentUserId))
                .Select(x => x.Messages.OrderByDescending(y => y.CreatedOn)
                    .FirstOrDefault());

            return sentBoxMessages;
        }

        private IQueryable<Message> GetInboxMessagesByUserId(string currentUserId)
        {

            var inboxMessages = this.context
                .Ads
                .Where(x => x.Messages.Any(y => y.RecipientId == currentUserId))
                .Select(x => x.Messages.OrderBy(y => y.CreatedOn)
                    .FirstOrDefault());
;
            return inboxMessages;
        }
    }
}
