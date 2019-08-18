namespace SellMe.Services
{
    using SellMe.Services.Interfaces;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using SellMe.Data;
    using SellMe.Web.ViewModels.ViewModels.Statistics;

    public class StatisticsService : IStatisticsService
    {
        private readonly SellMeDbContext context;

        public StatisticsService(SellMeDbContext context)
        {
            this.context = context;
        }

        public async Task<AdministrationIndexStatisticViewModel> GetAdministrationIndexStatisticViewModel()
        {
            var allUsersCount = await this.context.Users.CountAsync();
            var allAdsCount = await this.context.Ads.CountAsync();
            var allPromotedAds = await this.context
                .Ads
                .CountAsync(x => x.PromotionOrders.Any(y => y.IsActive && y.AdId == x.Id));

            var administrationIndexStatisticViewModel = new AdministrationIndexStatisticViewModel
            {
                AllAdsCount = allAdsCount,
                AllUsersCount = allUsersCount,
                AllPromotedAdsCount = allPromotedAds
            };

            return administrationIndexStatisticViewModel;
        }
    }
}
