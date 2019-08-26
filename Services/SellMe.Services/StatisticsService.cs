namespace SellMe.Services
{
    using SellMe.Services.Interfaces;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using SellMe.Data;
    using SellMe.Web.ViewModels.ViewModels.Statistics;
    using System;
    using System.Collections.Generic;
    using SellMe.Web.Infrastructure.Models;

    public class StatisticsService : IStatisticsService
    {
        private const int CreatedAdsStatisticDaysCount = 10;

        private readonly SellMeDbContext context;
        private readonly IAdsService adsService;
        private readonly IUsersService usersService;

        public StatisticsService(SellMeDbContext context, IAdsService adsService, IUsersService usersService)
        {
            this.context = context;
            this.adsService = adsService;
            this.usersService = usersService;
        }

        public async Task<AdministrationIndexStatisticViewModel> GetAdministrationIndexStatisticViewModel()
        {
            var allUsersCount = await this.usersService.GetCountOfAllUsersAsync();
            var allAdsCount = await this.adsService.GetAllActiveAdsCountAsync();

            var administrationIndexStatisticViewModel = new AdministrationIndexStatisticViewModel
            {
                AllAdsCount = allAdsCount,
                AllUsersCount = allUsersCount,
            };

            return administrationIndexStatisticViewModel;
        }

        public async Task<IEnumerable<DataPoint>> GetPointsForCreatedAds()
        {
            var lsatTenDates = this.GetLastTenDaysAsString();
            var countOfCreatedAds = await this.adsService.GetTheCountForTheCreatedAdsForTheLastTenDaysAsync();

            var dataPoints = new List<DataPoint>();

            for (int i = 0; i < CreatedAdsStatisticDaysCount; i++)
            {
                var dataPoint = new DataPoint(countOfCreatedAds[i], lsatTenDates[i]);
                dataPoints.Add(dataPoint);
            }

            return dataPoints;
        }

        private List<string> GetLastTenDaysAsString()
        {
            var dates = new List<string>();

            for (DateTime dt = DateTime.UtcNow.AddDays(-CreatedAdsStatisticDaysCount + 1); dt <= DateTime.UtcNow; dt = dt.AddDays(1))
            {
                dates.Add(dt.ToString("dd MMM"));
            }

            return dates;
        }
    }
}
