﻿namespace SellMe.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;
    using SellMe.Services.Interfaces;
    using SellMe.Web.ViewModels.Products;

    public class ProductsController : Controller
    {
        private readonly IProductService productService;

        public ProductsController(IProductService productService)
        {
            this.productService = productService;
        }

        public IActionResult Create()
        {
            return this.View();
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
    }
}
