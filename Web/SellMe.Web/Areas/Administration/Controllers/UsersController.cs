namespace SellMe.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;
    using Common;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Services.Interfaces;

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
            var allUserViewModels = await usersService.GetAllUserViewModelsAsync();

            return View(allUserViewModels);
        }

        [HttpPost]
        [Area("Administration")]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> Block(string userId)
        {
            var isBlocked = await usersService.BlockUserByIdAsync(userId);

            return Json(isBlocked);
        }
    }
}
