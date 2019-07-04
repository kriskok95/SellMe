using SellMe.Web.ViewModels.BindingModels.Messages;

namespace SellMe.Services
{
    using AutoMapper;
    using SellMe.Data;
    using SellMe.Data.Models;
    using SellMe.Services.Interfaces;
    using SellMe.Web.ViewModels.ViewModels.Messages;
    using SellMe.Web.ViewModels.BindingModels;
    using SellMe.Web.ViewModels.InputModels.Messages;

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
    }
}
