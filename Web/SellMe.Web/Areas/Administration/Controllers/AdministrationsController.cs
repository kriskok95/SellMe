namespace SellMe.Web.Areas.Administration.Controllers
{
    using SellMe.Services.Interfaces;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using SellMe.Common;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    public class AdministrationsController : Controller
    {
        private readonly IStatisticsService statisticsService;

        private readonly JsonSerializerSettings jsonSetting;

        public AdministrationsController(IStatisticsService statisticsService)
        {
            this.statisticsService = statisticsService;
            this.jsonSetting = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore };
        }

        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        [Area("Administration")]
        public async Task<IActionResult> Index()
        {
            var administrationIndexStatisticViewModel = await this.statisticsService.GetAdministrationIndexStatisticViewModel();

            var adsByDaysStatisticPoints = await this.statisticsService.GetPointsForCreatedAds();

            ViewBag.DataPoints = JsonConvert.SerializeObject(adsByDaysStatisticPoints, this.jsonSetting);

            return this.View(administrationIndexStatisticViewModel);
        }
    }
}
