namespace SellMe.Web.Hubs
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.SignalR;
    using Services.Interfaces;
    using ViewModels.InputModels.Messages;

    [Authorize]
    public class MessageHub : Hub
    {
        private readonly IMessagesService messagesService;

        public MessageHub(IMessagesService messagesService)
        {
            this.messagesService = messagesService;
        }

        public async Task UserMessagesCount(string userId)
        {
            var messagesCount = await messagesService.GetUnreadMessagesCountAsync(userId);

            await Clients.User(userId).SendAsync("MessageCount", messagesCount);
        }

        public async Task SendMessage(SendMessageInputModel inputModel)
        {
            var messageViewModel = await messagesService.CreateMessageAsync(inputModel.SenderId, inputModel.RecipientId, inputModel.AdId, inputModel.Content);

            await Clients.Users(inputModel.RecipientId)
                .SendAsync("SendMessage", messageViewModel);
        }
    }
}
