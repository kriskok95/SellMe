namespace SellMe.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Castle.Core.Internal;
    using Common;
    using Data;
    using Data.Models;
    using Interfaces;
    using Mapping;
    using Microsoft.EntityFrameworkCore;
    using Web.ViewModels.BindingModels.Messages;
    using Web.ViewModels.ViewModels.Messages;

    public class MessagesService : IMessagesService
    {
        private const string NotParticipantInConversationErrorMessage = "You are not participant in this conversation!";

        private readonly IAdsService adsService;
        private readonly IMapper mapper;
        private readonly IUsersService usersService;
        private readonly SellMeDbContext context;

        public MessagesService(SellMeDbContext context, IAdsService adsService, IUsersService usersService, IMapper mapper)
        {
            this.adsService = adsService;
            this.mapper = mapper;
            this.usersService = usersService;
            this.context = context;
        }

        public async Task<SendMessageViewModel> GetMessageViewModelByAdIdAsync(int adId)
        {
            if (!await context.Ads.AnyAsync(x => x.Id == adId))
            {
                throw new ArgumentException(GlobalConstants.InvalidAdIdErrorMessage);
            }

            var ad = await adsService.GetAdByIdAsync(adId);

            var sendMessageViewModel = mapper.Map<SendMessageViewModel>(ad);
            sendMessageViewModel.SenderId = usersService.GetCurrentUserId();

            return sendMessageViewModel;
        }

        public async Task<SendMessageBindingModel> GetMessageBindingModelByAdIdAsync(int adId)
        {
            if (!await context.Ads.AnyAsync(x => x.Id == adId))
            {
                throw new ArgumentException(GlobalConstants.InvalidAdIdErrorMessage);
            }

            var sendMessageViewModel = await GetMessageViewModelByAdIdAsync(adId);
            var sendMessageBindingModel = new SendMessageBindingModel
            {
                ViewModel = sendMessageViewModel
            };

            return sendMessageBindingModel;
        }

        public async Task<MessageDetailsViewModel> CreateMessageAsync(string senderId, string recipientId, int adId, string content)
        {
            if (!await context.Ads.AnyAsync(x => x.Id == adId))
            {
                throw new ArgumentException(GlobalConstants.InvalidAdIdErrorMessage);
            }

            var message = new Message
            {
                SenderId = senderId,
                RecipientId = recipientId,
                AdId = adId,
                Content = content
            };

            await context.Messages.AddAsync(message);
            await context.SaveChangesAsync();

            var messageFromDb = context.Messages.FirstOrDefault(x => x.Id == message.Id);

            var messageViewModel = mapper.Map<MessageDetailsViewModel>(messageFromDb);
            var sender = await usersService.GetUserByIdAsync(senderId);
            messageViewModel.Sender = sender.UserName;

            return messageViewModel;
        }



        public async Task<ICollection<MessageDetailsViewModel>> GetMessageDetailsViewModelsAsync(int adId, string senderId, string recipientId)
        {
            if (!await context.Ads.AnyAsync(x => x.Id == adId))
            {
                throw new ArgumentException(GlobalConstants.InvalidAdIdErrorMessage);
            }
            var currentUserId = usersService.GetCurrentUserId();

            if (currentUserId != senderId && currentUserId != recipientId)
            {
                throw new InvalidOperationException(NotParticipantInConversationErrorMessage);
            }

            var messagesFromFb = GetMessagesDetailsByAd(adId, senderId, recipientId);
            
            if (currentUserId == recipientId)
            {
                foreach (var message in messagesFromFb)
                {
                    message.IsRead = true;
                }
            }

            await context.SaveChangesAsync();

            var messageDetailsViewModels = await messagesFromFb
                .To<MessageDetailsViewModel>()
                .ToListAsync();

            return messageDetailsViewModels;
        }

        public async Task<IEnumerable<InboxMessageViewModel>> GetInboxMessagesViewModelsAsync()
        {
            var inboxMessageViewModels = await GetInboxViewModelsByCurrentUserAsync();

            return inboxMessageViewModels;
        }

        public async Task<SentBoxMessagesBindingModel> GetSentBoxMessagesBindingModelAsync()
        {
            var sentBoxMessageViewModel = await GetSentBoxViewModelByCurrentUserAsync();

            var sentBoxMessagesBindingModel = new SentBoxMessagesBindingModel
            {
                Messages = sentBoxMessageViewModel
            };

            return sentBoxMessagesBindingModel;
        }

        public async Task<int> GetUnreadMessagesCountAsync(string userId)
        {
            if (userId.IsNullOrEmpty())
            {
                throw new ArgumentException(GlobalConstants.InvalidUserIdErrorMessage);
            }

            var unreadMessagesCount = await context
                .Messages
                .CountAsync(x => x.RecipientId == userId && !x.IsRead);

            return unreadMessagesCount;
        }

        private IQueryable<Message> GetMessagesDetailsByAd(int adId, string senderId, string sellerId)
        {
            var messagesFromDb = context.Messages
                .Where(x => x.AdId == adId && (x.SenderId == senderId || x.SenderId == sellerId) && (x.RecipientId == sellerId || x.RecipientId == senderId))
                .OrderBy(date => date.CreatedOn);

            return messagesFromDb;
        }

        private IQueryable<Message> GetSentBoxMessagesByUserId(string currentUserId)
        {
            var sentBoxMessages = context
                .Ads
                .Where(x => x.Messages.Any(y => y.SenderId == currentUserId))
                .Select(x => x.Messages.OrderByDescending(y => y.CreatedOn)
                    .FirstOrDefault());

            return sentBoxMessages;
        }

        private IQueryable<Message> GetInboxMessagesByUserId(string currentUserId)
        {
            var inboxMessages = context
                .Ads
                .Where(x => x.Messages.Any(y => y.RecipientId == currentUserId))
                .Select(x => x.Messages.OrderByDescending(y => y.CreatedOn)
                    .FirstOrDefault());


            return inboxMessages;
        }

        private async Task<ICollection<InboxMessageViewModel>> GetInboxViewModelsByCurrentUserAsync()
        {
            var currentUserId = usersService.GetCurrentUserId();

            if (currentUserId == null)
            {
                throw new InvalidOperationException(GlobalConstants.UserIsNotLogInErrorMessage);
            }

            var inboxMessagesFromDb = GetInboxMessagesByUserId(currentUserId);

            await context.SaveChangesAsync();

            var inboxMessageViewModels = await inboxMessagesFromDb
                .OrderByDescending(x => x.CreatedOn)
                .To<InboxMessageViewModel>()
                .ToListAsync();

            return inboxMessageViewModels;
        }

        private async Task<ICollection<SentBoxMessageViewModel>> GetSentBoxViewModelByCurrentUserAsync()
        {
            var currentUserId = usersService.GetCurrentUserId();

            var sentBoxMessagesFromDb = GetSentBoxMessagesByUserId(currentUserId);
            var sentBoxMessageViewModels = await sentBoxMessagesFromDb
                .OrderByDescending(x => x.CreatedOn)
                .To<SentBoxMessageViewModel>()
                .ToListAsync();

            return sentBoxMessageViewModels;
        }
    }
}
