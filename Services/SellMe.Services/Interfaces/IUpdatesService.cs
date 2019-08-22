namespace SellMe.Services.Interfaces
{
    using System.Threading.Tasks;

    public interface IUpdatesService
    {
        Task CreateUpdateAdAsync(int adId);
    }
}
