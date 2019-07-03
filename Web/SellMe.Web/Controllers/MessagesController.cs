namespace SellMe.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using SellMe.Services.Interfaces;
    using SellMe.Web.ViewModels.BindingModels;
    using SellMe.Web.ViewModels.InputModels.Messages;

    public class MessagesController : Controller
    {
        private readonly IMessagesService messagesService;

        public MessagesController(IMessagesService messagesService)
        {
            this.messagesService = messagesService;
        }

        public IActionResult Send(int id)
        {
            var sendMessageBindingModel = this.messagesService.GetMessageBindingModelByAdId(id);

            return this.View(sendMessageBindingModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Send(SendMessageInputModel inputModel)
        {
            this.messagesService.CreateMessage(inputModel);

            return this.Redirect("/");
        }
    }
}
