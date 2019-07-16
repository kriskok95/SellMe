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
        public IActionResult Send(int id)
        {
            var sendMessageBindingModel = this.messagesService.GetMessageBindingModelByAdId(id);

            return this.View(sendMessageBindingModel);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult Send(SendMessageInputModel inputModel)
        {
            this.messagesService.CreateMessage(inputModel);

            return this.RedirectToAction("Details", new{ adId = inputModel.AdId, senderId = inputModel.SenderId, sellerId = inputModel.RecipientId});
        }

        public IActionResult Inbox()
        {
            var inboxMessageViewModels = this.messagesService
                .GetInboxViewModelsByCurrentUser()
                .ToList();

            return this.View(inboxMessageViewModels);
        }

        public IActionResult SentBox()
        {
            var sentBoxMessageViewModel = this.messagesService
                .GetSentBoxViewModelByCurrentUser()
                .ToList();

            return this.View(sentBoxMessageViewModel);
        }

        public IActionResult Details(MessageDetailsBindingModel bindingModel)
        {
            bindingModel.ViewModels = this.messagesService.GetMessageDetailsViewModels(bindingModel.AdId, bindingModel.SenderId, bindingModel.SellerId).ToList();
            bindingModel.AdTitle = this.adsService.GetAdTitleById(bindingModel.AdId);

            return this.View(bindingModel);
        }
    }
}
