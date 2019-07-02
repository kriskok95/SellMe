using Microsoft.AspNetCore.Mvc;
using SellMe.Services.Interfaces;

namespace SellMe.Web.Controllers
{
    public class MessagesController : Controller
    {
        private readonly IMessagesService messagesService;

        public MessagesController(IMessagesService messagesService)
        {
            this.messagesService = messagesService;
        }

        public IActionResult Send(int id)
        {
            var sendMessageViewModel = this.messagesService.GetMessageViewModelByAdId(id);

            return this.View();
        }
    }
}
