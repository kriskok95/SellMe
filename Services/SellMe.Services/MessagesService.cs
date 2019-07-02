using AutoMapper;

namespace SellMe.Services
{
    using SellMe.Services.Interfaces;
    using SellMe.Web.ViewModels.ViewModels.Messages;

    public class MessagesService : IMessagesService
    {
        private readonly IAdsService adsService;
        private readonly IMapper mapper;

        public MessagesService(IAdsService adsService, IMapper mapper)
        {
            this.adsService = adsService;
            this.mapper = mapper;
        }

        public SendMessageViewModel GetMessageViewModelByAdId(int adId)
        {
            var ad = this.adsService.GetAdById(adId);

            if (ad == null)
            {
                return null;
            }

            var sendMessageViewModel = mapper.Map<SendMessageViewModel>(ad);

            return sendMessageViewModel;
        }
    }
}
