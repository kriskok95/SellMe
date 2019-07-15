namespace SellMe.Services
{
    using AutoMapper;
    using SellMe.Data;
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

        public SendMessageViewModel GetMessageViewModelByAdId(int adId)
        {
            var ad = this.adsService.GetAdById(adId);

            if (ad == null)
            {
                return null;
            }

            var sendMessageViewModel = mapper.Map<SendMessageViewModel>(ad);
            sendMessageViewModel.SenderId = this.usersService.GetCurrentUserId();

            return sendMessageViewModel;
        }

        public SendMessageBindingModel GetMessageBindingModelByAdId(int adId)
        {
            var sendMessageViewModel = this.GetMessageViewModelByAdId(adId);
            var sendMessageBindingModel = new SendMessageBindingModel
            {
                ViewModel = sendMessageViewModel
            };

            return sendMessageBindingModel;
        }

        public void CreateMessage(SendMessageInputModel inputModel)
        {
            var message = this.mapper.Map<Message>(inputModel);

            this.context.Messages.Add(message);
            this.context.SaveChanges();
        }

        public ICollection<InboxMessageViewModel> GetInboxViewModelsByCurrentUser()
        {
            var currentUserId = this.usersService.GetCurrentUserId();

            var inboxMessagesFromDb = this.GetInboxMessagesByUserId(currentUserId);
            var inboxMessageViewModels = inboxMessagesFromDb
                .To<InboxMessageViewModel>()
                .ToList();

            return inboxMessageViewModels;
        }

        public ICollection<SentBoxMessageViewModel> GetSentBoxViewModelByCurrentUser()
        {
            var currentUserId = this.usersService.GetCurrentUserId();

            var sentBoxMessagesFromDb = this.GetSentBoxMessagesByUserId(currentUserId);
            var sentBoxMessageViewModels = sentBoxMessagesFromDb
                .To<SentBoxMessageViewModel>()
                .ToList();

            return sentBoxMessageViewModels;
        }

        public ICollection<MessageDetailsViewModel> GetMessageDetailsViewModels(int adId, string senderId, string recipientId)
        {
            var messagesFromFb = this.GetMessagesDetailsByAd(adId, senderId, recipientId);

            var messageDetailsViewModels = messagesFromFb
                .To<MessageDetailsViewModel>()
                .ToList();

            return messageDetailsViewModels;
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
                .Select(x => x.Messages.OrderByDescending(y => y.CreatedOn).FirstOrDefault());

            return sentBoxMessages;
        }

        private IQueryable<Message> GetInboxMessagesByUserId(string currentUserId)
        {

            var inboxMessages = this.context
                .Ads
                .Where(x => x.Messages.Any(y => y.RecipientId == currentUserId))
                .Select(x => x.Messages.OrderBy(y => y.CreatedOn).FirstOrDefault());
;
            return inboxMessages;
        }
    }
}
