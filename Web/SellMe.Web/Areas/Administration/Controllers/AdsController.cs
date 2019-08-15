namespace SellMe.Web.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using SellMe.Common;
    using Microsoft.AspNetCore.Mvc;
    using SellMe.Services.Interfaces;
    using System.Threading.Tasks;

    public class AdsController : Controller
    {
        private readonly IAdsService adsService;

        public AdsController(IAdsService adsService)
        {
            this.adsService = adsService;
        }

        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        [Area("Administration")]
        public async Task<IActionResult> Approvement()
        {
            var adsForApprovementViewModels = await this.adsService.GetAdsForApprovementViewModels();

            return null;
        }
    }
}
