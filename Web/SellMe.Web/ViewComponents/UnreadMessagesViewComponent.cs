namespace SellMe.Web.ViewComponents
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using SellMe.Services.Interfaces;
    using SellMe.Web.ViewModels.ViewModels.Notifications;

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
            var userId = this.usersService.GetCurrentUserId();

            var unreadMessagesCount = await this.messagesService.GetUnreadMessagesCountAsync(userId);

            var notificationViewModel = new NotificationViewModel {Count = unreadMessagesCount};

            return this.View(notificationViewModel);
        }
    }
}
