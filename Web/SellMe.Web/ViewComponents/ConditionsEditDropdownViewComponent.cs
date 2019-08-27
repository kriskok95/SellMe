namespace SellMe.Web.ViewComponents
{
    using Microsoft.AspNetCore.Mvc;
    using Services.Interfaces;

    public class ConditionsEditDropdownViewComponent : ViewComponent
    {
        private readonly IConditionsService conditionsService;

        public ConditionsEditDropdownViewComponent(IConditionsService conditionsService)
        {
            this.conditionsService = conditionsService;
        }

        public IViewComponentResult Invoke(string adCondition)
        {
            var conditionsViewModels = conditionsService.GetConditionViewModels();
            ViewData["adCondition"] = adCondition;
            return View(conditionsViewModels);
        }
    }
}
