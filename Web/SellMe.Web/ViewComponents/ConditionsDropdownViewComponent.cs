﻿namespace SellMe.Web.ViewComponents
{
    using Microsoft.AspNetCore.Mvc;
    using SellMe.Services.Interfaces;

    public class ConditionsDropdownViewComponent : ViewComponent
    {
        private readonly IConditionsService conditionsService;

        public ConditionsDropdownViewComponent(IConditionsService conditionsService)
        {
            this.conditionsService = conditionsService;
        }

        public IViewComponentResult Invoke()
        {
            var conditionsViewModels = this.conditionsService.GetConditionViewModels();
            return this.View(conditionsViewModels);
        }
    }
}