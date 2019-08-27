namespace SellMe.Web.ViewComponents
{
    using Microsoft.AspNetCore.Mvc;
    using Services.Interfaces;

    public class ConditionsDropdownViewComponent : ViewComponent
    {
        private readonly IConditionsService conditionsService;

        public ConditionsDropdownViewComponent(IConditionsService conditionsService)
        {
            this.conditionsService = conditionsService;
        }

        public IViewComponentResult Invoke()
        {
            var conditionsViewModels = conditionsService.GetConditionViewModels();
            return View(conditionsViewModels);
        }
    }
}
