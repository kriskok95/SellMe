using SellMe.Web.ViewModels.InputModels.Messages;

namespace SellMe.Web.Hubs
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.SignalR;
    using SellMe.Services.Interfaces;
    using System.Threading.Tasks;

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
            var messagesCount = await this.messagesService.GetUnreadMessagesCountAsync(userId);

            await this.Clients.User(userId).SendAsync("MessageCount", messagesCount);
        }

        public async Task SendMessage(SendMessageInputModel inputModel)
        {
            var messageViewModel = await this.messagesService.CreateMessageAsync(inputModel.SenderId, inputModel.RecipientId, inputModel.AdId, inputModel.Content);

            await this.Clients.Users(inputModel.RecipientId)
                .SendAsync("SendMessage", messageViewModel);
        }
    }
}
