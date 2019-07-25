﻿namespace SellMe.Web.ViewModels.BindingModels.Messages
{
    using SellMe.Web.ViewModels.InputModels.Messages;
    using SellMe.Web.ViewModels.ViewModels.Messages;
    using SellMe.Web.ViewModels.ViewModels;

    public class SendMessageBindingModel : BaseViewModel
    {
        public SendMessageViewModel ViewModel { get; set; }

        public SendMessageInputModel InputModel { get; set; }
    }
}
