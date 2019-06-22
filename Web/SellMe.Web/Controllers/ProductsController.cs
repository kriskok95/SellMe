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
                Categories = categoryNames.Select(x => x).ToList()
            };


            return this.View(categoryNamesViewModel);
        }
    }
}
