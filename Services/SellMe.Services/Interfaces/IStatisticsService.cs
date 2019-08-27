namespace SellMe.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Web.Infrastructure.Models;
    using Web.ViewModels.ViewModels.Statistics;

    public interface IStatisticsService
    {
        Task<AdministrationIndexStatisticViewModel> GetAdministrationIndexStatisticViewModel();

        Task<IEnumerable<DataPoint>> GetPointsForCreatedAds();
    }
}
