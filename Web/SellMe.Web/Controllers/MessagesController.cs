using System.Threading.Tasks;

namespace SellMe.Web.Controllers
{
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

        public MessagesController(IMessagesService messagesService, IAdsService adsService)
        {
            this.messagesService = messagesService;
            this.adsService = adsService;
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
