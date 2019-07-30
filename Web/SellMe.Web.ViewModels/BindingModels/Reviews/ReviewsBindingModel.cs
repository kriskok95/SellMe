﻿namespace SellMe.Web.ViewModels.BindingModels.Reviews
{
    using SellMe.Web.ViewModels.InputModels.Reviews;
    using SellMe.Web.ViewModels.ViewModels;
    using System.Collections.Generic;
    using SellMe.Web.ViewModels.ViewModels.Reviews;

    public class ReviewsBindingModel : BaseViewModel
    {
        public string OwnerId { get; set; }

        public string SenderId { get; set; }

        public string OwnerUsername{ get; set; }

        public ReviewInputModel InputModel { get; set; }

        public IEnumerable<ReviewViewModel> ViewModels { get; set; }
    }
}