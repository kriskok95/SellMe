using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SellMe.Data;
using SellMe.Data.Models;

namespace SellMe.Services
{
    using SellMe.Services.Interfaces;

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

        private async void CreateSellMeUserFavoriteProductAsync(int adId, string currentUserId)
        {
            var sellMeUserFavoriteProduct = new SellMeUserFavoriteProduct()
            {
                AdId = adId,
                SellMeUserId = currentUserId
            };

            await this.context.SellMeUserFavoriteProducts.AddAsync(sellMeUserFavoriteProduct);

            await this.context.SaveChangesAsync();
        }
    }
}
