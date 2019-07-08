namespace SellMe.Services.Interfaces
{
    public interface IFavoritesService
    {
        bool AddToFavorites(int adId);

        bool RemoveFromFavorites(int adId);
    }
}
