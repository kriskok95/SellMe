namespace SellMe.Web.Controllers
{
    using SellMe.Web.ViewModels.BindingModels.Ads;
    using Microsoft.AspNetCore.Mvc;
    using SellMe.Services.Interfaces;
    using SellMe.Web.ViewModels.InputModels.Ads;
    using SellMe.Web.ViewModels.ViewModels.Ads;
    using Microsoft.AspNetCore.Authorization;
    using System.Threading.Tasks;

    public class AdsController : Controller
    {
        private const int DefaultPageSize = 10;
        private const int DefaultPageNumber = 1;

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

        public async Task<IActionResult> All(int? pageNumber)
        {
            var adsAllViewModels = await this.adService.GetAllAdViewModelsAsync(pageNumber ?? DefaultPageNumber, DefaultPageSize);

            return this.View(adsAllViewModels);
        }

        public async Task<IActionResult> AdsByCategory(AdsByCategoryInputModel inputModel)
        {
            var adsByCategoryViewModel = await this.adService.GetAdsByCategoryViewModelAsync(inputModel.Id, inputModel.PageNumber?? DefaultPageNumber, DefaultPageSize);

             return this.View(adsByCategoryViewModel);
        }

        public async Task<IActionResult> Details(AdDetailsInputModel inputModel)
        {
            AdDetailsViewModel adDetailsViewModel = await this.adService.GetAdDetailsViewModelAsync(inputModel.Id);

            return this.View(adDetailsViewModel);
        }

        [Authorize]
        public async Task<IActionResult> ActiveAds(int? pageNumber)
        {
            var bindingModel = await this.adService.GetMyActiveAdsBindingModelAsync(pageNumber?? DefaultPageNumber, DefaultPageSize);

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
        public async Task<IActionResult> ArchivedAds(int? pageNumber)
        {

            var archivedAdsBindingModel = await this.adService.GetArchivedAdsBindingModelAsync(pageNumber ?? DefaultPageNumber, DefaultPageSize);


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

        [HttpPost]
        public async Task<IActionResult> Edit(EditAdBindingModel bindingModel)
        {
            await this.adService.EditAdById(bindingModel.EditAdInputModel);
            return this.Redirect("/");
        }

        public async Task<IActionResult> Update(UpdateAdInputModel inputModel)
        {
            await this.adService.UpdateAdByIdAsync(inputModel.AdId);

            return this.RedirectToAction("ActiveAds");
        }

        public async Task<IActionResult> AdsBySubcategory(AdsBySubcategoryInputModel inputModel)
        {
            //TODO: Create input model
            var adsBySubcategoryViewModel = await this.adService.GetAdsBySubcategoryViewModelAsync(inputModel.SubcategoryId, inputModel.CategoryId, inputModel.PageNumber?? DefaultPageNumber, DefaultPageSize);

            return this.View(adsBySubcategoryViewModel);
        }

        public async Task<IActionResult> BySearch(AdsBySearchInputModel inputModel)
        {
            var adsBySearchBindingModel = await this.adService.GetAdsBySearchViewModelAsync(inputModel.Search);

             return this.View(adsBySearchBindingModel);
        }

        public async Task<IActionResult> AdsByUser(string userId)
        {
            var adsByUserBindingModel = await this.adService.GetAdsByUserBindingModelAsync(userId);

            return this.View(adsByUserBindingModel);
        }

        public async Task<IActionResult> WaitingForApproval()
        {
            var waitingForApprovalAdsByUser = await this.adService.GetWaitingForApprovalByCurrentUserViewModels();

            return this.View(waitingForApprovalAdsByUser);
        }

        public async Task<IActionResult> Rejected()
        {
            var rejectedAdsByUser = await this.adService.GetRejectedAdByUserViewModelsAsync();

            return this.View(rejectedAdsByUser);
        }
    }
}
