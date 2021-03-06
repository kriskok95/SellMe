﻿namespace SellMe.Web.Controllers
{
    using System.Threading.Tasks;
    using Data.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Services.Interfaces;

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

        [Authorize]
        public async Task<IActionResult> MyFavorites(int? pageNumber)
        {
            var loggedInUserId = userManager.GetUserId(User);

            var favoriteAdsViewModels = await adsService.GetFavoriteAdsViewModelsAsync(loggedInUserId, pageNumber?? DefaultPageNumber, DefaultPageSize);

            return View(favoriteAdsViewModels);
        }

        [Authorize]
        public async Task<IActionResult> Add(int adId)
        {
            bool isAdded = await favoritesService.AddToFavoritesAsync(adId);

            return Json(isAdded);
        }

        [Authorize]
        public async Task<IActionResult> Remove(int adId)
        {
            bool isRemoved = await favoritesService.RemoveFromFavoritesAsync(adId);

            return Json(isRemoved);
        }
    }
}
