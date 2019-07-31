namespace SellMe.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Identity;
    using SellMe.Data.Models;
    using SellMe.Services.Interfaces;
    using System;
    using System.Threading.Tasks;
    using System.Linq;


    public class FavoritesController : Controller
    {
        private const int DefaultPageSize = 10;
        private const int DefaultPageNumber = 1;

        private readonly IAdsService adsService;
        private readonly UserManager<SellMeUser> userManager;
        private readonly IFavoritesService favoritesService;


        public FavoritesController(IAdsService adsService, UserManager<SellMeUser> userManager, IFavoritesService favoritesService)
        {
            this.adsService = adsService;
            this.userManager = userManager;
            this.favoritesService = favoritesService;
        }

        public async Task<IActionResult> MyFavorites(int? pageNumber)
        {
            var loggedInUserId = this.userManager.GetUserId(this.User);

            var favoriteAdsBindingModel = await this.adsService.GetFavoriteAdsBindingModelAsync(loggedInUserId, pageNumber?? DefaultPageNumber, DefaultPageSize);

            return this.View(favoriteAdsBindingModel);
        }

        public async Task<IActionResult> Add(int adId)
        {
            bool isAdded = await this.favoritesService.AddToFavoritesAsync(adId);

            return Json(isAdded);
        }

        public async Task<IActionResult> Remove(int adId)
        {
            bool isRemoved = await this.favoritesService.RemoveFromFavoritesAsync(adId);

            return Json(isRemoved);
        }
    }
}
