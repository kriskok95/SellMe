﻿namespace SellMe.Web.ViewComponents
{
    using Microsoft.AspNetCore.Mvc;
    using SellMe.Web.ViewModels.ViewModels.Ads;

    public class LatestAddedAdIndexPageViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(LatestAddedAdViewModel viewModel)
        {
            var result = viewModel.Title.Length > 28 ? viewModel.Title.Substring(0, 25) + "..." : viewModel.Title;
            viewModel.Title = result;

            return this.View(viewModel);
        }
    }
}
