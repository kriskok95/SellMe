﻿namespace SellMe.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;
    using SellMe.Services.Interfaces;
    using SellMe.Web.ViewModels.InputModels.Ads;
    using SellMe.Web.ViewModels.ViewModels.Ads;
    using Microsoft.AspNetCore.Authorization;

    public class AdsController : Controller
    {
        private readonly IAdsService adService;
        private readonly ISubCategoriesService subCategoriesService;


        public AdsController(IAdsService adService, ISubCategoriesService subCategoriesService)
        {
            this.adService = adService;
            this.subCategoriesService = subCategoriesService;
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateAdInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            this.adService.CreateAd(inputModel);

            return this.Redirect("/");
        }

        public IActionResult GetSubcategories(int categoryId)
        {
            var subcategories = this.subCategoriesService
                .GetSubcategoriesByCategoryId(categoryId);

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

        [Authorize]
        public IActionResult MyAds()
        {
            var myAdsViewModels = this.adService
                .GetMyAdsViewModels()
                .ToList();

            return this.View(myAdsViewModels);
        }

        public IActionResult ArchivedAds(int adId)
        {
            bool isArchived = this.adService.ArchiveAdById(adId);

            var result = new
            {
                success = true
            };

            return Json(result);
        }
    }
}
