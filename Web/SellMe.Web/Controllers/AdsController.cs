namespace SellMe.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;
    using SellMe.Services.Interfaces;
    using SellMe.Web.ViewModels.InputModels.Ads;
    using SellMe.Web.ViewModels.ViewModels.Ads;
    using Microsoft.AspNetCore.Authorization;
    using System.Threading.Tasks;

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
        public async Task<IActionResult> Create(CreateAdInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            await this.adService.CreateAdAsync(inputModel);

            return this.Redirect("/");
        }

        public async Task<IActionResult> GetSubcategoriesAsync(int categoryId)
        {
            var subcategories = await this.subCategoriesService
                .GetSubcategoriesByCategoryIdAsync(categoryId);

            return Json(subcategories);
        }

        public async Task<IActionResult> All()
        {
            var adsAllViewModels = await this.adService.GetAllAdViewModelsAsync();

            return this.View(adsAllViewModels);
        }

        public async Task<IActionResult> AdsByCategory(AdsByCategoryInputModel inputModel)
        {
            var adsByCategoryViewModel = await this.adService.GetAdsByCategoryViewModelAsync(inputModel.Id);

             return this.View(adsByCategoryViewModel);
        }

        public async Task<IActionResult> Details(AdDetailsInputModel inputModel)
        {
            AdDetailsViewModel adDetailsViewModel = await this.adService.GetAdDetailsViewModelAsync(inputModel.Id);

            return this.View(adDetailsViewModel);
        }

        [Authorize]
        public async Task<IActionResult> ActiveAds()
        {
            var myAdsViewModels = await this.adService
                .GetMyAdsViewModelsAsync();


            return this.View(myAdsViewModels.ToList());
        }

        [Authorize]
        public async Task<IActionResult> ArchiveAd(int adId)
        {
            //TODO: Change the name to ArchiveAdAsync
            bool isArchived = await this.adService.ArchiveAdByIdAsync(adId);

            var result = new
            {
                success = true
            };

            return Json(result);
        }

        [Authorize]
        public async Task<IActionResult> ArchivedAds()
        {
            var myArchivedAds = await this.adService
                .GetMyArchivedAdsViewModelsAsync();


            return this.View(myArchivedAds.ToList());
        }

        [Authorize]
        public async Task<IActionResult> ActivateAd(int adId)
        {
            //TODO: Change the name to activateAdAsync
            bool isActivated = await this.adService.ActivateAdById(adId);

            var result = new
            {
                success = true
            };

            return Json(result);
        }

        [Authorize]
        public IActionResult Edit(int id)
        {
            var editAdBindingModel = this.adService.GetEditAdBindingModelById(id);

            return this.View(editAdBindingModel);
        }

    }
}
