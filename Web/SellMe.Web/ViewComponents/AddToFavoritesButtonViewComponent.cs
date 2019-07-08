 namespace SellMe.Web.ViewComponents
{
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using SellMe.Services.Interfaces;

    public class AddToFavoritesButtonViewComponent : ViewComponent
    {
        private readonly IUsersService usersService;

        public AddToFavoritesButtonViewComponent(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        public IViewComponentResult Invoke(int adId)
        {
            var currentUser = this.usersService.GetCurrentUser();

            if (currentUser.SellMeUserFavoriteProducts.Any(x => x.AdId == adId))
            {
                return this.View("Remove", adId);
            }
            else
            {
                return this.View("Add", adId);  
            }
        }
    }
}
