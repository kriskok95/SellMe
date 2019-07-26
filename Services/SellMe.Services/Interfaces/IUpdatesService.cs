namespace SellMe.Services.Interfaces
{
    using System.Threading.Tasks;

    public interface IUpdatesService
    {
        Task CreateUpdateAd(int adId);
    }
}
