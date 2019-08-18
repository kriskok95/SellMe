namespace SellMe.Web.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using SellMe.Services.Interfaces;
    using Microsoft.AspNetCore.Authorization;
    using SellMe.Common;

    public class UsersController : Controller
    {
        private readonly IUsersService usersService;

        public UsersController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        [Area("Administration")]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> All()
        {
            var allUserViewModels = await this.usersService.GetAllUserViewModelsAsync();

            return this.View(allUserViewModels);
        }
    }
}
