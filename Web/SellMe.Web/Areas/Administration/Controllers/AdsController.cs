using SellMe.Web.ViewModels.InputModels.Ads;

namespace SellMe.Web.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using SellMe.Common;
    using Microsoft.AspNetCore.Mvc;
    using SellMe.Services.Interfaces;
    using System.Threading.Tasks;
    using System.Diagnostics.CodeAnalysis;

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
        public async Task<IActionResult> ForApproval()
        {
            var adsForApprovementViewModels = await this.adsService.GetAdsForApprovalViewModelsAsync();

            return this.View(adsForApprovementViewModels);
        }

        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        [Area("Administration")]
        public async Task<IActionResult> Approve(int adId)
        {
            var result = await this.adsService.ApproveAdAsync(adId);

            return Json(result);
        }

        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        [Area("Administration")]
        public async Task<IActionResult> RejectAd(int adId)
        {
            var rejectAdViewModel = await this.adsService.GetRejectAdBindingModelAsync(adId);

            return this.View(rejectAdViewModel);
        }

        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        [Area("Administration")]
        [HttpPost]
        public async Task<IActionResult> RejectAd(RejectAdInputModel inputModel)
        {
            await this.adsService.CreateAdRejectionAsync(inputModel.AdId, inputModel.Comment);
            var adsForApprovementViewModels = await this.adsService.GetAdsForApprovalViewModelsAsync();

            return this.RedirectToAction("ForApproval", adsForApprovementViewModels);
        }

        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        [Area("Administration")]
        public async Task<IActionResult> RejectedAds()
        {
            var rejectedAdsViewModels = await this.adsService.GetRejectedAdAllViewModelsAsync();

            return this.View(rejectedAdsViewModels);
        }

        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        [Area("Administration")]
        public async Task<IActionResult> AllActiveAds(int? pageNumber)
        {
            var allActiveAdViewModel = await this.adsService.GetAllActiveAdViewModelsAsync(pageNumber ?? DefaultPageNumber, DefaultPageSize);

            return this.View(allActiveAdViewModel);
        }

        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        [Area("Administration")]
        public async Task<IActionResult> ArchiveAd(int adId)
        {
            bool isArchived = await this.adsService.ArchiveAdByIdAsync(adId);

            var result = new
            {
                success = true
            };

            return Json(result);
        }
    }
}
