﻿namespace SellMe.Web.ViewComponents
{
    using Microsoft.AspNetCore.Mvc;
    using ViewModels.ViewModels.Ads;

    public class ListedAdViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(AdViewModel viewModel)
        {
            var result = viewModel.Description.Length > 220 ? viewModel.Description.Substring(0, 217) + "..." : viewModel.Description;
            viewModel.Description = result;

            return View(viewModel);
        }
    }
}
