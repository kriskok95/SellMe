namespace SellMe.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Common;
    using Data;
    using Interfaces;
    using Web.Infrastructure.Models;
    using Web.ViewModels.ViewModels.Statistics;

    public class StatisticsService : IStatisticsService
    {
        private readonly SellMeDbContext context;
        private readonly IAdsService adsService;
        private readonly IUsersService usersService;
        private readonly IPromotionsService promotionsService;

        public StatisticsService(SellMeDbContext context, IAdsService adsService, IUsersService usersService, IPromotionsService promotionsService)
        {
            this.context = context;
            this.adsService = adsService;
            this.usersService = usersService;
            this.promotionsService = promotionsService;
        }

        public async Task<AdministrationIndexStatisticViewModel> GetAdministrationIndexStatisticViewModel()
        {
            var allUsersCount = await usersService.GetCountOfAllUsersAsync();
            var allAdsCount = await adsService.GetAllActiveAdsCountAsync();

            var administrationIndexStatisticViewModel = new AdministrationIndexStatisticViewModel
            {
                AllAdsCount = allAdsCount,
                AllUsersCount = allUsersCount
            };

            return administrationIndexStatisticViewModel;
        }

        public async Task<IEnumerable<DataPoint>> GetPointsForCreatedAdsAsync()
        {
            var lsatTenDates = this.GetLastTenDaysAsString();
            var countOfCreatedAds = await adsService.GetTheCountForTheCreatedAdsForTheLastTenDaysAsync();

            var dataPoints = new List<DataPoint>();

            for (int i = 0; i < GlobalConstants.CreatedAdsStatisticDaysCount; i++)
            {
                var dataPoint = new DataPoint(countOfCreatedAds[i], lsatTenDates[i]);
                dataPoints.Add(dataPoint);
            }

            return dataPoints;
        }

        public async Task<IEnumerable<DataPoint>> GetPointsForPromotionsAsync()
        {
            var lastTenDates = this.GetLastTenDaysAsString();
            var countOfPromotions = await this.promotionsService.GetTheCountOfPromotionsForTheLastTenDaysAsync();

            var dataPoints = new List<DataPoint>();

            for (int i = 0; i < GlobalConstants.PromotionsBoughtStatisticDaysCount; i++)
            {
                var dataPoint = new DataPoint(countOfPromotions[i], lastTenDates[i]);
                dataPoints.Add(dataPoint);
            }

            return dataPoints;
        }

        private List<string> GetLastTenDaysAsString()
        {
            var dates = new List<string>();

            for (DateTime dt = DateTime.UtcNow.AddDays(-GlobalConstants.CreatedAdsStatisticDaysCount + 1); dt <= DateTime.UtcNow; dt = dt.AddDays(1))
            {
                dates.Add(dt.ToString("dd MMM"));
            }

            return dates;
        }
    }
}
