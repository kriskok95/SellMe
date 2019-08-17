namespace SellMe.Web.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using SellMe.Common;
    using Microsoft.AspNetCore.Mvc;
    using SellMe.Services.Interfaces;
    using System.Threading.Tasks;
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("ReSharper", "IdentifierTypo")]
    public class AdsController : Controller
    {
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
    }
}
