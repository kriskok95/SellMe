namespace SellMe.Web.ViewComponents
{
    using Microsoft.AspNetCore.Mvc;

    using SellMe.Services.Interfaces;
    public class ConditionsEditDropdownViewComponent : ViewComponent
    {
        private readonly IConditionsService conditionsService;

        public ConditionsEditDropdownViewComponent(IConditionsService conditionsService)
        {
            this.conditionsService = conditionsService;
        }

        public IViewComponentResult Invoke(string adCondition)
        {
            var conditionsViewModels = this.conditionsService.GetConditionViewModels();
            ViewData["adCondition"] = adCondition;
            return this.View(conditionsViewModels);
        }
    }
}
