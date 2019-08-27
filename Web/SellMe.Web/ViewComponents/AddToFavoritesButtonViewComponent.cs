  namespace SellMe.Web.ViewComponents
{
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Services.Interfaces;

    public class AddToFavoritesButtonViewComponent : ViewComponent
    {
        private readonly IUsersService usersService;

        public AddToFavoritesButtonViewComponent(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int adId)
        {
            var currentUser = await usersService.GetCurrentUserAsync();

            if (currentUser.SellMeUserFavoriteProducts.Any(x => x.AdId == adId))
            {
                return View("Remove", adId);
            }

            return View("Add", adId);
        }
    }
}
