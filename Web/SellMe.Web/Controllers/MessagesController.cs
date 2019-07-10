using System.Linq;

namespace SellMe.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using SellMe.Services.Interfaces;
    using SellMe.Web.ViewModels.InputModels.Messages;
    using Microsoft.AspNetCore.Authorization;

    public class MessagesController : Controller
    {
        private readonly IMessagesService messagesService;

        public MessagesController(IMessagesService messagesService)
        {
            this.messagesService = messagesService;
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

            return this.Redirect("/");
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
        
    }
}
