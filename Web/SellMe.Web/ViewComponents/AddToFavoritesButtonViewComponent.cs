 using System.Threading.Tasks;

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

        public async Task<IViewComponentResult> InvokeAsync(int adId)
        {
            var currentUser = await this.usersService.GetCurrentUserAsync();

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
