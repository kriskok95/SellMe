namespace SellMe.Services.Interfaces
{
    using System.Threading.Tasks;
    using Web.ViewModels.ViewModels.Home;

    public interface IHomeService
    {
        Task<IndexViewModel> GetIndexViewModelAsync();
    }
}
