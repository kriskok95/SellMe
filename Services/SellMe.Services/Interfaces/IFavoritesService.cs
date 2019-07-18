namespace SellMe.Services.Interfaces
{
    using System.Threading.Tasks;

    public interface IFavoritesService
    {
        Task<bool> AddToFavoritesAsync(int adId);

        Task<bool> RemoveFromFavoritesAsync(int adId);
    }
}
