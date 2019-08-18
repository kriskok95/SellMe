namespace SellMe.Services.Interfaces
{
    using System.Threading.Tasks;
    using SellMe.Web.ViewModels.ViewModels.Statistics;

    public interface IStatisticsService
    {
        Task<AdministrationIndexStatisticViewModel> GetAdministrationIndexStatisticViewModel();
    }
}
