namespace SellMe.Web.ViewComponents
{
    using Microsoft.AspNetCore.Mvc;
    using ViewModels.ViewModels.Ads;

    public class PromotedAdIndexPageViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(PromotedAdViewModel viewModel)
        {

        
                var result = viewModel.Title.Length > 28 ? viewModel.Title.Substring(0, 25) + "..." : viewModel.Title;
                viewModel.Title = result;
            

            return View(viewModel);
        }
    }
}
