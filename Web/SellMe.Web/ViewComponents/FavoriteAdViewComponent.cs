namespace SellMe.Web.ViewComponents
{
    using Microsoft.AspNetCore.Mvc;
    using SellMe.Web.ViewModels.ViewModels.Ads;

    public class FavoriteAdViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(FavoriteAdViewModel viewModel)
        {
            var result = viewModel.Description.Length > 220 ? viewModel.Description.Substring(0, 217) + "..." : viewModel.Description;
            viewModel.Description = result;

            return this.View(viewModel);
        }
    }
}
