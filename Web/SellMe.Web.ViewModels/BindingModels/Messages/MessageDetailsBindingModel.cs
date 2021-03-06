﻿namespace SellMe.Web.ViewModels.BindingModels.Messages
{
    using System.Collections.Generic;
    using InputModels.Messages;
    using ViewModels;
    using ViewModels.Messages;

    public class MessageDetailsBindingModel : BaseViewModel
    {
        public string RecipientId { get; set; }

        public string SenderId { get; set; }

        public int AdId { get; set; }

        public string AdTitle { get; set; }

        public ICollection<MessageDetailsViewModel> ViewModels { get; set; }

        public MessageDetailsInputModel InputModel { get; set; }
    }
}
