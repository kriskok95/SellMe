namespace SellMe.Services
{
    using SellMe.Services.Interfaces;
    using System.Linq;
    using SellMe.Data;
    using SellMe.Data.Models;


    public class FavoritesService : IFavoritesService
    {
        private readonly IUsersService usersService;
        private readonly SellMeDbContext context;

        public FavoritesService(SellMeDbContext context, IUsersService usersService)
        {
            this.context = context;
            this.usersService = usersService;
        }

        public bool AddToFavorites(int adId)
        {
            var currentUser = this.usersService.GetCurrentUser();

            if (currentUser == null)
            {
                return false;
            }

            var isInFavorites = currentUser.SellMeUserFavoriteProducts
                .Any(x => x.AdId == adId);

            if (isInFavorites)
            {
                return false;
            }

            this.CreateSellMeUserFavoriteProductAsync(adId, currentUser.Id);
            return true;
        }

        public bool RemoveFromFavorites(int adId)
        {
            var currentUser = this.usersService.GetCurrentUser();

            if (currentUser == null)
            {
                return false;
            }

            var isInFavorites = currentUser.SellMeUserFavoriteProducts
                .Any(x => x.AdId == adId);

            if (!isInFavorites)
            {
                return false;
            }

            this.RemoveSellMeUserFavoriteProduct(adId, currentUser.Id);
            return true;
        }

        private void RemoveSellMeUserFavoriteProduct(int adId, string currentUserId)
        {
            var sellMeUserFavoriteProduct = this.context.SellMeUserFavoriteProducts.First(x => x.AdId == adId && x.SellMeUserId == currentUserId);

            this.context.Remove(sellMeUserFavoriteProduct);
            this.context.SaveChanges();
        }

        private void CreateSellMeUserFavoriteProductAsync(int adId, string currentUserId)
        {
            var sellMeUserFavoriteProduct = new SellMeUserFavoriteProduct()
            {
                AdId = adId,
                SellMeUserId = currentUserId
            };

           this.context.SellMeUserFavoriteProducts.Add(sellMeUserFavoriteProduct);
           this.context.SaveChanges();
        }
    }
}
