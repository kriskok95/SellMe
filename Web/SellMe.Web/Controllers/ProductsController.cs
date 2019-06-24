using System.Linq;
using SellMe.Services.Interfaces;
using SellMe.Web.ViewModels.Products;

namespace SellMe.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class ProductsController : Controller
    {
        private readonly IProductService productService;

        public ProductsController(IProductService productService)
        {
            this.productService = productService;
        }

        public IActionResult Create()
        {
            var categoryNames = this.productService.GetCategoryNames();

            var categoryNamesViewModel = new CreateProductViewModel()
            {
                Categories = categoryNames.Select(x => x).ToList(),
            };


            return this.View(categoryNamesViewModel);
        }

        public IActionResult GetSubcategories(string categoryName)
        {
            var subcategories = this.productService
                .GetSubcategoriesByCategory(categoryName)
                .Select(x => new
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToList();

            return Json(subcategories);
        }
    }
}
