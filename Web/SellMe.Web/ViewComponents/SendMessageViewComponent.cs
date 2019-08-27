
namespace SellMe.Web.ViewComponents
{
    using Microsoft.AspNetCore.Mvc;
    using Services.Interfaces;
    using ViewModels.InputModels.Messages;

    public class SendMessageViewComponent : ViewComponent
    {
        private readonly IUsersService usersService;

        public SendMessageViewComponent(IUsersService usersService)
        {
            this.usersService = usersService;
        }
        public IViewComponentResult Invoke(string user1Id, string user2Id, int adId)
        {
            var currentUserId = usersService.GetCurrentUserId();

            var senderId = string.Empty;
            var recipientId = string.Empty;

            if (currentUserId == user2Id)
            {
                senderId = user2Id;
                recipientId = user1Id;
            }
            else
            {
                senderId = user1Id;
                recipientId = user2Id;
            }

            var inputModel = new SendMessageInputModel
            {
                SenderId = senderId,
                RecipientId = recipientId,
                AdId = adId
            };

            return View(inputModel);
        }
    }
}
