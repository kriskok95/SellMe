namespace SellMe.Web.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using SellMe.Common;

    public class AdministrationsController : Controller
    {
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        [Area("Administration")]
        public IActionResult Index()
        {
            return this.View();
        }
    }
}
