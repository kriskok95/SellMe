using System.Threading.Tasks;

namespace SellMe.Web.Areas.Administration.Controllers
{
    using SellMe.Services.Interfaces;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using SellMe.Common;


    public class AdministrationsController : Controller
    {
        private readonly IStatisticsService statisticsService;

        public AdministrationsController(IStatisticsService statisticsService)
        {
            this.statisticsService = statisticsService;
        }

        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        [Area("Administration")]
        public async Task<IActionResult> Index()
        {
            var administrationIndexStatisticViewModel = await this.statisticsService.GetAdministrationIndexStatisticViewModel();

            return this.View(administrationIndexStatisticViewModel);
        }
    }
}
