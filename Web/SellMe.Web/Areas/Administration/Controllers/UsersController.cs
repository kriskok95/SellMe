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
        public async Task<IActionResult> Active(int? pageNumber)
        {
            var allUserViewModels = await usersService.GetAllUserViewModelsAsync(pageNumber ?? GlobalConstants.DefaultPageNumber, GlobalConstants.DefaultPageSize);

            return View(allUserViewModels);
        }

        [Area("Administration")]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> Blocked(int? pageNumber)
        {
            var allBlockedUserViewModels = await this.usersService.GetAllBlockedUserViewModels(pageNumber ?? GlobalConstants.DefaultPageNumber, GlobalConstants.DefaultPageSize);

            return this.View(allBlockedUserViewModels);
        }

        [HttpPost]
        [Area("Administration")]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> Block(string userId)
        {
            var isBlocked = await usersService.BlockUserByIdAsync(userId);

            return Json(isBlocked);
        }

        [HttpPost]
        [Area("Administration")]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> Unblock(string userId)
        {
            var isUnblocked = await this.usersService.UnblockUserByIdAsync(userId);

            return Json(isUnblocked);
        }
    }
}
