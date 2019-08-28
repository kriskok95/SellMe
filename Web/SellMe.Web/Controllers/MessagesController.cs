namespace SellMe.Web.Controllers
{
    using System.Threading.Tasks;
    using Hubs;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.SignalR;
    using Services.Interfaces;
    using ViewModels.BindingModels.Messages;
    using ViewModels.InputModels.Messages;

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
            var sendMessageBindingModel = await messagesService.GetMessageBindingModelByAdIdAsync(id);

            return View(sendMessageBindingModel);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Send(SendMessageInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                var sendMessageBindingModel =
                    await messagesService.GetMessageBindingModelByAdIdAsync(inputModel.AdId);
                sendMessageBindingModel.InputModel = inputModel;

                return View(sendMessageBindingModel);
            }

            var messageViewModel = await messagesService.CreateMessageAsync(inputModel.SenderId, inputModel.RecipientId,
                inputModel.AdId, inputModel.Content);

            var unreadMessagesCount = await messagesService.GetUnreadMessagesCountAsync(inputModel.RecipientId);

            await hubContext.Clients.User(inputModel.RecipientId).SendAsync("MessageCount", unreadMessagesCount);

            await hubContext.Clients.User(inputModel.RecipientId)
                .SendAsync("SendMessage", messageViewModel);

            return RedirectToAction("Details", new{ adId = inputModel.AdId, senderId = inputModel.SenderId, recipientId = inputModel.RecipientId});
        }

        public async Task<IActionResult> Inbox()
        {
            var inboxMessagesViewModels = await messagesService.GetInboxMessagesViewModelsAsync();

            return View(inboxMessagesViewModels);
        }

        public async Task<IActionResult> SentBox()
        {
            var sentBoxMessagesBindingModel = await messagesService.GetSentBoxMessagesBindingModelAsync();

            return View(sentBoxMessagesBindingModel);
        }

        [Authorize]
        public async Task<IActionResult> Details(MessageDetailsBindingModel bindingModel)
        {
            bindingModel.ViewModels = await messagesService.GetMessageDetailsViewModelsAsync(bindingModel.AdId, bindingModel.SenderId, bindingModel.RecipientId);
            bindingModel.AdTitle = await adsService.GetAdTitleByIdAsync(bindingModel.AdId);

            return View(bindingModel);
        }
    }
}
