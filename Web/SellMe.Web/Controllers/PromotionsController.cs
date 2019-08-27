namespace SellMe.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using SellMe.Services.Interfaces;
    using SellMe.Web.ViewModels.BindingModels.Promotions;
    using Microsoft.AspNetCore.Authorization;

    public class PromotionsController : Controller
    {
        private const string SuccessfullyPromotedMessage = "You have successfully promoted this ad.";

        private readonly IPromotionsService promotionsService;

        public PromotionsController(IPromotionsService promotionsService)
        {
            this.promotionsService = promotionsService;
        }

        public async Task<IActionResult> Index(int adId)
        {
            var bindingModel = await this.promotionsService.GetPromotionBindingModelByAdIdAsync(adId);

            return this.View(bindingModel);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Buy(PromotionBindingModel bindingModel)
        {
            await this.promotionsService.CreatePromotionOrderAsync(bindingModel.PromotionInputModel.AdId,
                bindingModel.PromotionInputModel.PromotionId);

            TempData["SuccessfullyPromotedMessage"] = SuccessfullyPromotedMessage;

            return this.RedirectToAction("Index", new{adId = bindingModel.PromotionInputModel.AdId});
        }
    }
}
