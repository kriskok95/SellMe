namespace SellMe.Web.ViewComponents
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Services.Interfaces;
    using ViewModels.ViewModels.Notifications;

    public class UnreadMessagesViewComponent : ViewComponent
    {
        private readonly IMessagesService messagesService;
        private readonly IUsersService usersService;

        public UnreadMessagesViewComponent(IMessagesService messagesService, IUsersService usersService)
        {
            this.messagesService = messagesService;
            this.usersService = usersService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId = usersService.GetCurrentUserId();

            var unreadMessagesCount = await messagesService.GetUnreadMessagesCountAsync(userId);

            var notificationViewModel = new NotificationViewModel {Count = unreadMessagesCount};

            return View(notificationViewModel);
        }
    }
}
