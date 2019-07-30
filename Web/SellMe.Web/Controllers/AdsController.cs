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
            var bindingModel = await this.adService.GetMyActiveAdsBindingModelAsync();

            return this.View(bindingModel);
        }

        [Authorize]
        public async Task<IActionResult> ArchiveAd(int adId)
        {
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

            var archivedAdsBindingModel = await this.adService.GetArchivedAdsBindingModelAsync();


            return this.View(archivedAdsBindingModel);
        }

        [Authorize]
        public async Task<IActionResult> ActivateAd(int adId)
        {
            bool isActivated = await this.adService.ActivateAdById(adId);

            var result = new
            {
                success = true
            };

            return Json(result);
        }

        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var editAdBindingModel = await this.adService.GetEditAdBindingModelById(id);

            return this.View(editAdBindingModel);
        }

        public async Task<IActionResult> Update(UpdateAdInputModel inputModel)
        {
            await this.adService.UpdateAdByIdAsync(inputModel.AdId);

            return this.RedirectToAction("ActiveAds");
        }

        public async Task<IActionResult> AdsBySubcategory(int subcategoryId, int categoryId)
        {
            var adsBySubcategoryViewModel = await this.adService.GetAdsBySubcategoryViewModelAsync(subcategoryId, categoryId);

            return this.View(adsBySubcategoryViewModel);
        }

        public async Task<IActionResult> BySearch(AdsBySearchInputModel inputModel)
        {
            var adsBySearchBindingModel = await this.adService.GetAdsBySearchViewModelAsync(inputModel.Search);

             return this.View(adsBySearchBindingModel);
        }

        public async Task<IActionResult> AdsByUser(string userId)
        {
            var adsByUserBindingModel = await this.adService.GetAdsByUserBindingModel(userId);

            return this.View(adsByUserBindingModel);
        }
    }
}
