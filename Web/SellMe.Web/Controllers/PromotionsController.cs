namespace SellMe.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using SellMe.Services.Interfaces;
    using SellMe.Web.ViewModels.BindingModels.Promotions;
    using Microsoft.AspNetCore.Authorization;

    public class PromotionsController : Controller
    {
        private readonly IPromotionsService promotionsService;

        public PromotionsController(IPromotionsService promotionsService)
        {
            this.promotionsService = promotionsService;
        }

        public async Task<IActionResult> Index(int adId)
        {
            var viewModel = await this.promotionsService.GetPromotionBindingModelByAdIdAsync(adId);

            return this.View(viewModel);
        }
        
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Buy(PromotionBindingModel bindingModel)
        {
            await this.promotionsService.CreatePromotionForAdAsync(bindingModel.PromotionInputModel.AdId,
                bindingModel.PromotionInputModel.PromotionType);

            return this.RedirectToAction("ActiveAds", "Ads");
        }
    }
}
