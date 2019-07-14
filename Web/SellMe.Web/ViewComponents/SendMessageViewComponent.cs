
namespace SellMe.Web.ViewComponents
{
    using SellMe.Web.ViewModels.InputModels.Messages;
    using Microsoft.AspNetCore.Mvc;
    using SellMe.Services.Interfaces;

    public class SendMessageViewComponent : ViewComponent
    {
        private readonly IUsersService usersService;

        public SendMessageViewComponent(IUsersService usersService)
        {
            this.usersService = usersService;
        }
        public IViewComponentResult Invoke(string userId, string sellerId, int adId)
        {
            var currentUserId = this.usersService.GetCurrentUserId();

            var senderId = string.Empty;
            var recipientId = string.Empty;

            if (currentUserId == sellerId)
            {
                senderId = sellerId;
                recipientId = userId;
            }
            else
            {
                senderId = userId;
                recipientId = sellerId;
            }

            var inputModel = new SendMessageInputModel()
            {
                SenderId =  senderId,
                RecipientId = recipientId,
                AdId = adId,
            };

            return this.View(inputModel);
        }
    }
}
