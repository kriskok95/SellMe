namespace SellMe.Web.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.SignalR;
    using SellMe.Web.Hubs;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using SellMe.Services.Interfaces;
    using SellMe.Web.ViewModels.InputModels.Messages;
    using Microsoft.AspNetCore.Authorization;
    using SellMe.Web.ViewModels.BindingModels.Messages;

    public class MessagesController : Controller
    {
        private readonly IMessagesService messagesService;
        private readonly IAdsService adsService;
        private readonly IHubContext<MessageHub> hubContext;

        public MessagesController(IMessagesService messagesService, IAdsService adsService, IHubContext<MessageHub> hubContext)
        {
            this.messagesService = messagesService;
            this.adsService = adsService;
            this.hubContext = hubContext;
        }

        [Authorize]
        public async Task<IActionResult> Send(int id)
        {
            var sendMessageBindingModel = await this.messagesService.GetMessageBindingModelByAdIdAsync(id);

            return this.View(sendMessageBindingModel);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Send(SendMessageInputModel inputModel)
        {
            await this.messagesService.CreateMessageAsync(inputModel);

            var unreadMessagesCount = await this.messagesService.GetUnreadMessagesCountAsync(inputModel.RecipientId);

            await this.hubContext.Clients.User(inputModel.RecipientId).SendAsync("MessageCount", unreadMessagesCount);

            return this.RedirectToAction("Details", new{ adId = inputModel.AdId, senderId = inputModel.SenderId, sellerId = inputModel.RecipientId});
        }

        public async Task<IActionResult> Inbox()
        {
            var inboxMessagesBindingModel = await this.messagesService.GetInboxMessagesBindingModelAsync();

            return this.View(inboxMessagesBindingModel);
        }

        public async Task<IActionResult> SentBox()
        {
            var sentBoxMessagesBindingModel = await this.messagesService.GetSentBoxMessagesBindingModelAsync();

            return this.View(sentBoxMessagesBindingModel);
        }

        public async Task<IActionResult> Details(MessageDetailsBindingModel bindingModel)
        {
            bindingModel.ViewModels = await this.messagesService.GetMessageDetailsViewModelsAsync(bindingModel.AdId, bindingModel.SenderId, bindingModel.SellerId);
            bindingModel.AdTitle = this.adsService.GetAdTitleById(bindingModel.AdId);

            return this.View(bindingModel);
        }
    }
}
