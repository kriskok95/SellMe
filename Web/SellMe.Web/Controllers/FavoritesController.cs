namespace SellMe.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Identity;
    using SellMe.Data.Models;
    using SellMe.Services.Interfaces;

    public class FavoritesController : Controller
    {
        private readonly IAdsService adsService;
        private readonly UserManager<SellMeUser> userManager;


        public FavoritesController(IAdsService adsService, UserManager<SellMeUser> userManager)
        {
            this.adsService = adsService;
            this.userManager = userManager;
        }

        public IActionResult MyFavorites()
        {
            var loggedInUserId = this.userManager.GetUserId(this.User);

            var favoriteAdViewModels = this.adsService.GetFavoriteAdsByUserId(loggedInUserId);

            return this.View();
        }
    }
}
