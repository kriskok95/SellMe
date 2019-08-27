namespace SellMe.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;
    using Common;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using Services.Interfaces;

    public class HomeController : Controller
    {
        private readonly IStatisticsService statisticsService;

        private readonly JsonSerializerSettings jsonSetting;

        public HomeController(IStatisticsService statisticsService)
        {
            this.statisticsService = statisticsService;
            jsonSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
        }

        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        [Area("Administration")]
        public async Task<IActionResult> Index()
        {
            var administrationIndexStatisticViewModel = await statisticsService.GetAdministrationIndexStatisticViewModel();

            var adsByDaysStatisticPoints = await this.statisticsService.GetPointsForCreatedAdsAsync();
            var promotionsByDaysStatisticPoints = await this.statisticsService.GetPointsForPromotionsAsync();

            ViewBag.DataPointsAds = JsonConvert.SerializeObject(adsByDaysStatisticPoints, jsonSetting);
            ViewBag.DataPointsPromotions = JsonConvert.SerializeObject(promotionsByDaysStatisticPoints, jsonSetting);

            return View(administrationIndexStatisticViewModel);
        }
    }
}
