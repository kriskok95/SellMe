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

        private const string SuccessfullyCreatedAdMessage =
            "Your ad was successfully created. It will be reviewed by an administrator and approved as soon as possible!";

        private const string SuccessfullyUpdatedAdMessage =
            "You ad was successfully updated and moved at the top of the page!";

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

            TempData["CreatedAd"] = SuccessfullyCreatedAdMessage;
                

            return this.RedirectToAction("WaitingForApproval");
        }

        public async Task<IActionResult> GetSubcategoriesAsync(int categoryId)
        {
            var subcategories = await this.subCategoriesService
                .GetSubcategoriesByCategoryIdAsync(categoryId);

            return Json(subcategories);
        }

        public async Task<IActionResult> AdsByCategory(AdsByCategoryInputModel inputModel)
        {
            var adsByCategoryViewModel = await this.adService.GetAdsByCategoryViewModelAsync(inputModel.Id, inputModel.PageNumber?? DefaultPageNumber, DefaultPageSize);

             return this.View(adsByCategoryViewModel);
        }

        public async Task<IActionResult> Details(AdDetailsInputModel inputModel)
        {
            var adDetailsViewModel = await this.adService.GetAdDetailsViewModelAsync(inputModel.Id);

            return this.View(adDetailsViewModel);
        }

        [Authorize]
        public async Task<IActionResult> ActiveAds(int? pageNumber)
        {
            var viewModel = await this.adService.GetMyActiveAdsViewModelsAsync(pageNumber?? DefaultPageNumber, DefaultPageSize);

            return this.View(viewModel);
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

            var archivedAdsViewModels = await this.adService.GetArchivedAdsViewModelsAsync(pageNumber ?? DefaultPageNumber, DefaultPageSize);


            return this.View(archivedAdsViewModels);
        }

        [Authorize]
        public async Task<IActionResult> ActivateAd(int adId)
        {
            bool isActivated = await this.adService.ActivateAdByIdAsync(adId);

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
            await this.adService.EditAd(bindingModel.EditAdInputModel);
            return this.Redirect("/");
        }

        public async Task<IActionResult> Update(UpdateAdInputModel inputModel)
        {
            await this.adService.UpdateAdByIdAsync(inputModel.AdId);

            TempData["SuccessfulUpdateMessage"] = SuccessfullyUpdatedAdMessage;

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
            var adsBySearchViewModels = await this.adService.GetAdsBySearchViewModelsAsync(inputModel.Search, DefaultPageNumber, DefaultPageSize);

             return this.View(adsBySearchViewModels);
        }

        public async Task<IActionResult> AdsByUser(string userId, int? pageNumber)
        {
            var adsByUserBindingModel = await this.adService.GetAdsByUserBindingModelAsync(userId, pageNumber ?? DefaultPageNumber, DefaultPageSize);

            return this.View(adsByUserBindingModel);
        }

        public async Task<IActionResult> WaitingForApproval(int? pageNumber)
        {
            var waitingForApprovalAdsByUser = await this.adService.GetWaitingForApprovalByCurrentUserViewModels(pageNumber ?? DefaultPageNumber, DefaultPageSize);

            return this.View(waitingForApprovalAdsByUser);
        }

        public async Task<IActionResult> Rejected(int? pageNumber)
        {
            var rejectedAdsByUser = await this.adService.GetRejectedAdByUserViewModelsAsync(pageNumber ?? DefaultPageNumber, DefaultPageSize);

            return this.View(rejectedAdsByUser);
        }

        public async Task<IActionResult> SubmitRejectedAd(int rejectionId)
        {
            var isSucceeded = await this.adService.SubmitRejectedAdAsync(rejectionId);

            return this.Json(isSucceeded);
        }
    }
}
