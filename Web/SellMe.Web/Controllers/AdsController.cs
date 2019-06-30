namespace SellMe.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;
    using SellMe.Services.Interfaces;
    using SellMe.Web.ViewModels.InputModels.Ads;
    using SellMe.Web.ViewModels.ViewModels.Ads;

    public class AdsController : Controller
    {
        private readonly IAdsService adService;
       

        public AdsController(IAdsService adService)
        {
            this.adService = adService;
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

            this.adService.CreateAd(inputModel);

            return this.Redirect("/");
        }

        public IActionResult GetSubcategories(string categoryName)
        {
            var subcategories = this.adService
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
            var adsAllViewModels = this.adService.GetAllAdViewModels();

            return this.View(adsAllViewModels);
        }

        public IActionResult AdsByCategory(AdsByCategoryInputModel inputModel)
        {
            var adsByCategoryViewModel = this.adService.GetAdsByCategoryViewModel(inputModel.Id);

             return this.View(adsByCategoryViewModel);
        }

        public IActionResult Details(AdDetailsInputModel inputModel)
        {
            AdDetailsViewModel adDetailsViewModel = this.adService.GetAdDetailsViewModel(inputModel.Id);

            return this.View(adDetailsViewModel);
        }
    }
}
