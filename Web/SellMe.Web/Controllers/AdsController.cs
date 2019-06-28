using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace SellMe.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;
    using SellMe.Services.Interfaces;
    using SellMe.Web.ViewModels.InputModels.Products;

    public class AdsController : Controller
    {
        private readonly IAdsService _adService;
       

        public AdsController(IAdsService adService)
        {
            this._adService = adService;
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Create(CreateAdInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            this._adService.CreateProduct(inputModel);

            return this.Redirect("/");
        }

        public IActionResult GetSubcategories(string categoryName)
        {
            var subcategories = this._adService
                .GetSubcategoriesByCategory(categoryName)
                .Select(x => new
                {
                    Name = x.Name
                })
                .ToList();

            return Json(subcategories);
        }

        public IActionResult All()
        {
            var allProductsViewModel = this._adService.GetAllProductsViewModels().ToList();

            return this.View(allProductsViewModel);
        }
    }
}
