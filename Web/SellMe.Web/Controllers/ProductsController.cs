using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace SellMe.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;
    using SellMe.Services.Interfaces;
    using SellMe.Web.ViewModels.InputModels.Products;

    public class ProductsController : Controller
    {
        private readonly IProductsService productService;
       

        public ProductsController(IProductsService productService)
        {
            this.productService = productService;
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

            this.productService.CreateProduct(inputModel);

            return this.Redirect("/");
        }

        public IActionResult GetSubcategories(string categoryName)
        {
            var subcategories = this.productService
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
            var allProductsViewModel = this.productService.GetAllProductsViewModels().ToList();

            return this.View(allProductsViewModel);
        }
    }
}
