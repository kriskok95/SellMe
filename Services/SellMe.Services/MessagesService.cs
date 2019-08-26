namespace SellMe.Services
{
    using Castle.Core.Internal;
    using SellMe.Common;
    using System;
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

        public MessagesService(SellMeDbContext context, IAdsService adsService, IUsersService usersService, IMapper mapper)
        {
            this.adsService = adsService;
            this.mapper = mapper;
            this.usersService = usersService;
            this.context = context;
        }

        public async Task<SendMessageViewModel> GetMessageViewModelByAdIdAsync(int adId)
        {
            if (!await this.context.Ads.AnyAsync(x => x.Id == adId))
            {
                throw new ArgumentException(GlobalConstants.InvalidAdIdErrorMessage);
            }

            var ad = await this.adsService.GetAdByIdAsync(adId);

            var sendMessageViewModel = mapper.Map<SendMessageViewModel>(ad);
            sendMessageViewModel.SenderId = this.usersService.GetCurrentUserId();

            return sendMessageViewModel;
        }

        public async Task<SendMessageBindingModel> GetMessageBindingModelByAdIdAsync(int adId)
        {
            if (!await this.context.Ads.AnyAsync(x => x.Id == adId))
            {
                throw new ArgumentException(GlobalConstants.InvalidAdIdErrorMessage);
            }

            var sendMessageViewModel = await this.GetMessageViewModelByAdIdAsync(adId);
            var sendMessageBindingModel = new SendMessageBindingModel
            {
                ViewModel = sendMessageViewModel
            };

            return sendMessageBindingModel;
        }

        public async Task<MessageDetailsViewModel> CreateMessageAsync(string senderId, string recipientId, int adId, string content)
        {
            if (!await this.context.Ads.AnyAsync(x => x.Id == adId))
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

            await this.context.Messages.AddAsync(message);
            await this.context.SaveChangesAsync();

            var messageFromDb = this.context.Messages.FirstOrDefault(x => x.Id == message.Id);

            var messageViewModel = mapper.Map<MessageDetailsViewModel>(messageFromDb);
            var sender = await this.usersService.GetUserByIdAsync(senderId);
            messageViewModel.Sender = sender.UserName;

            return messageViewModel;
        }



        public async Task<ICollection<MessageDetailsViewModel>> GetMessageDetailsViewModelsAsync(int adId, string senderId, string recipientId)
        {
            if (!await this.context.Ads.AnyAsync(x => x.Id == adId))
            {
                throw new ArgumentException(GlobalConstants.InvalidAdIdErrorMessage);
            }

            var messagesFromFb = this.GetMessagesDetailsByAd(adId, senderId, recipientId);
            var currentUserId = this.usersService.GetCurrentUserId();
            if (currentUserId == recipientId)
            {
                foreach (var message in messagesFromFb)
                {
                    message.IsRead = true;
                }
            }

            await this.context.SaveChangesAsync();

            var messageDetailsViewModels = await messagesFromFb
                .To<MessageDetailsViewModel>()
                .ToListAsync();

            return messageDetailsViewModels;
        }

        public async Task<IEnumerable<InboxMessageViewModel>> GetInboxMessagesViewModelsAsync()
        {
            var inboxMessageViewModels = await this.GetInboxViewModelsByCurrentUserAsync();

            return inboxMessageViewModels;
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

        public async Task<int> GetUnreadMessagesCountAsync(string userId)
        {
            if (userId.IsNullOrEmpty())
            {
                throw new ArgumentException(GlobalConstants.InvalidUserIdErrorMessage);
            }

            var unreadMessagesCount = await this.context
                .Messages
                .CountAsync(x => x.RecipientId == userId && !x.IsRead);

            return unreadMessagesCount;
        }

        private IQueryable<Message> GetMessagesDetailsByAd(int adId, string senderId, string sellerId)
        {
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
                .Select(x => x.Messages.OrderByDescending(y => y.CreatedOn)
                    .FirstOrDefault());


            return inboxMessages;
        }

        private async Task<ICollection<InboxMessageViewModel>> GetInboxViewModelsByCurrentUserAsync()
        {
            var currentUserId = this.usersService.GetCurrentUserId();

            if (currentUserId == null)
            {
                throw new InvalidOperationException(GlobalConstants.UserIsNotLogInErrorMessage);
            }

            var inboxMessagesFromDb = this.GetInboxMessagesByUserId(currentUserId);

            await this.context.SaveChangesAsync();

            var inboxMessageViewModels = await inboxMessagesFromDb
                .OrderByDescending(x => x.CreatedOn)
                .To<InboxMessageViewModel>()
                .ToListAsync();

            return inboxMessageViewModels;
        }

        private async Task<ICollection<SentBoxMessageViewModel>> GetSentBoxViewModelByCurrentUserAsync()
        {
            var currentUserId = this.usersService.GetCurrentUserId();

            var sentBoxMessagesFromDb = this.GetSentBoxMessagesByUserId(currentUserId);
            var sentBoxMessageViewModels = await sentBoxMessagesFromDb
                .OrderByDescending(x => x.CreatedOn)
                .To<SentBoxMessageViewModel>()
                .ToListAsync();

            return sentBoxMessageViewModels;
        }
    }
}
