namespace SellMe.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Identity;
    using SellMe.Data.Models;
    using SellMe.Services.Interfaces;
    using System;
    using System.Linq;


    public class FavoritesController : Controller
    {
        private readonly IAdsService adsService;
        private readonly UserManager<SellMeUser> userManager;
        private readonly IFavoritesService favoritesService;


        public FavoritesController(IAdsService adsService, UserManager<SellMeUser> userManager, IFavoritesService favoritesService)
        {
            this.adsService = adsService;
            this.userManager = userManager;
            this.favoritesService = favoritesService;
        }

        public IActionResult MyFavorites()
        {
            var loggedInUserId = this.userManager.GetUserId(this.User);

            var favoriteAdViewModels = this.adsService.GetFavoriteAdsByUserIdAsync(loggedInUserId)
                .GetAwaiter()
                .GetResult()
                .ToList();

            return this.View(favoriteAdViewModels);
        }

        public IActionResult Add(int adId)
        {
            bool isAdded = this.favoritesService.AddToFavorites(adId);

            return Json(isAdded);
        }

        public IActionResult Remove(int adId)
        {
            bool isRemoved = this.favoritesService.RemoveFromFavorites(adId);

            return Json(isRemoved);
        }
    }
}
