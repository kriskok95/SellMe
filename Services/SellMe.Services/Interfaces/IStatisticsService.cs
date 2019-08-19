using System.Collections.Generic;
using SellMe.Web.Infrastructure.Models;

namespace SellMe.Services.Interfaces
{
    using System.Threading.Tasks;
    using SellMe.Web.ViewModels.ViewModels.Statistics;

    public interface IStatisticsService
    {
        Task<AdministrationIndexStatisticViewModel> GetAdministrationIndexStatisticViewModel();

        Task<IEnumerable<DataPoint>> GetPointsForCreatedAds();
    }
}
