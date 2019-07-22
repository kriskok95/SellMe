namespace SellMe.Services.Interfaces
{
    using SellMe.Web.ViewModels.ViewModels.Home;
    using System.Threading.Tasks;

    public interface IHomeService
    {
        Task<IndexViewModel> GetIndexViewModel();
    }
}
