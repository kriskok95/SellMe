namespace SellMe.Web.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Services.Interfaces;
    using ViewModels.BindingModels.Promotions;

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
            var bindingModel = await promotionsService.GetPromotionBindingModelByAdIdAsync(adId);

            return View(bindingModel);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Buy(PromotionBindingModel bindingModel)
        {
            await promotionsService.CreatePromotionOrderAsync(bindingModel.PromotionInputModel.AdId,
                bindingModel.PromotionInputModel.PromotionId);

            TempData["SuccessfullyPromotedMessage"] = SuccessfullyPromotedMessage;

            return RedirectToAction("Index", new{adId = bindingModel.PromotionInputModel.AdId});
        }
    }
}
