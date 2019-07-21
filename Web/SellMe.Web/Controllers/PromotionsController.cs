using AutoMapper.Configuration.Conventions;

namespace SellMe.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class PromotionsController : Controller
    {
        public IActionResult Index(int adId)
        {
            

            return this.View();
        }
    }
}
