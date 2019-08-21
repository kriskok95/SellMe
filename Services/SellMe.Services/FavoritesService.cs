namespace SellMe.Services
{
    using System;
    using Microsoft.EntityFrameworkCore;

    using SellMe.Services.Interfaces;
    using System.Linq;
    using SellMe.Data;
    using SellMe.Data.Models;
    using System.Threading.Tasks;


    public class FavoritesService : IFavoritesService
    {
        private const string CurrentUserIsNullErrorMessage = "Current user can't be null";
        private const string InvalidAdIdErrorMessage = "Ad with the given id doesn't exist!";
        private const string AdIsAlreadyInFavoritesErrorMessage = "The given ad is already added to favorites!";
        private const string AdIsNotInFavoritesListErrorMessage = "The given ad isn't in the favorites list!";

        private readonly IUsersService usersService;
        private readonly SellMeDbContext context;

        public FavoritesService(SellMeDbContext context, IUsersService usersService)
        {
            this.context = context;
            this.usersService = usersService;
        }

        public async Task<bool> AddToFavoritesAsync(int adId)
        {
            var currentUser = await this.usersService.GetCurrentUserAsync();

            if (currentUser == null)
            {
                throw new InvalidOperationException(CurrentUserIsNullErrorMessage);
            }

            if (!await this.context.Ads.AnyAsync(x => x.Id == adId))
            {
                throw new ArgumentException(InvalidAdIdErrorMessage);
            }

            var isInFavorites = currentUser.SellMeUserFavoriteProducts
                .Any(x => x.AdId == adId);

            if (isInFavorites)
            {
                throw new InvalidOperationException(AdIsAlreadyInFavoritesErrorMessage);
            }

            await this.CreateSellMeUserFavoriteProductAsync(adId, currentUser.Id);
            return true;
        }

        public async Task<bool> RemoveFromFavoritesAsync(int adId)
        {
            var currentUser = await this.usersService.GetCurrentUserAsync();

            if (currentUser == null)
            {
                throw new InvalidOperationException(CurrentUserIsNullErrorMessage);
            }

            if (!await this.context.Ads.AnyAsync(x => x.Id == adId))
            {
                throw new ArgumentException(InvalidAdIdErrorMessage);
            }

            var isInFavorites = currentUser.SellMeUserFavoriteProducts
                .Any(x => x.AdId == adId);

            if (!isInFavorites)
            {
                throw new InvalidOperationException(AdIsNotInFavoritesListErrorMessage);
            }

            await this.RemoveSellMeUserFavoriteProductAsync(adId, currentUser.Id);
            return true;
        }
        
        private async Task RemoveSellMeUserFavoriteProductAsync(int adId, string currentUserId)
        {
            var sellMeUserFavoriteProduct = this.context.SellMeUserFavoriteProducts.First(x => x.AdId == adId && x.SellMeUserId == currentUserId);

            this.context.Remove(sellMeUserFavoriteProduct);
            await this.context.SaveChangesAsync();
        }

        private async Task CreateSellMeUserFavoriteProductAsync(int adId, string currentUserId)
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
