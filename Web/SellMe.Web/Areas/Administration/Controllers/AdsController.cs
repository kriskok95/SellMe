namespace SellMe.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;
    using Common;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Services.Interfaces;
    using ViewModels.InputModels.Ads;

    public class AdsController : Controller
    {
        private const int DefaultPageSize = 10;
        private const int DefaultPageNumber = 1;

        private readonly IAdsService adsService;

        public AdsController(IAdsService adsService)
        {
            this.adsService = adsService;
        }

        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        [Area("Administration")]
        public async Task<IActionResult> ForApproval(int? pageNumber)
        {
            var adsForApprovalViewModels = await adsService.GetAdsForApprovalViewModelsAsync(pageNumber ?? DefaultPageNumber, DefaultPageSize);

            return View(adsForApprovalViewModels);
        }

        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        [Area("Administration")]
        public async Task<IActionResult> Approve(int adId)
        {
            var result = await adsService.ApproveAdAsync(adId);

            return Json(result);
        }

        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        [Area("Administration")]
        public async Task<IActionResult> RejectAd(int adId)
        {
            var rejectAdViewModel = await adsService.GetRejectAdBindingModelAsync(adId);

            return View(rejectAdViewModel);
        }

        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        [Area("Administration")]
        [HttpPost]
        public async Task<IActionResult> RejectAd(RejectAdInputModel inputModel)
        {
            await adsService.CreateAdRejectionAsync(inputModel.AdId, inputModel.Comment);
            var adsForApprovalViewModels = await adsService.GetAdsForApprovalViewModelsAsync(DefaultPageNumber, DefaultPageSize);

            return RedirectToAction("ForApproval", adsForApprovalViewModels);
        }

        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        [Area("Administration")]
        public async Task<IActionResult> RejectedAds(int? pageNumber)
        {
            var rejectedAdsViewModels = await adsService.GetRejectedAdAllViewModelsAsync(pageNumber?? DefaultPageNumber, DefaultPageSize);

            return View(rejectedAdsViewModels);
        }

        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        [Area("Administration")]
        public async Task<IActionResult> AllActiveAds(int? pageNumber)
        {
            var allActiveAdViewModel = await adsService.GetAllActiveAdViewModelsAsync(pageNumber ?? DefaultPageNumber, DefaultPageSize);

            return View(allActiveAdViewModel);
        }

        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        [Area("Administration")]
        public async Task<IActionResult> ArchiveAd(int adId)
        {
            bool isArchived = await adsService.ArchiveAdByIdAsync(adId);

            var result = new
            {
                success = true
            };

            return Json(result);
        }
    }
}
