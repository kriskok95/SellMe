namespace SellMe.Web.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Services.Interfaces;
    using ViewModels.BindingModels.Ads;
    using ViewModels.InputModels.Ads;

    public class AdsController : Controller
    {
        private const int DefaultPageSize = 10;
        private const int DefaultPageNumber = 1;

        private const string SuccessfullyCreatedAdMessage =
            "Your ad was successfully created. It will be reviewed by an administrator and approved as soon as possible!";

        private const string SuccessfullyUpdatedAdMessage =
            "You ad was successfully updated and moved at the top of the page!";

        private const string SuccessfullyEditedAdsMessage = "You have successfully edited this ad!";

        private const string UnSuccessfullyUpdatesAdMessage = "You can't update the given ad because it hasn't any available updates!";

        private readonly IAdsService adService;
        private readonly ISubCategoriesService subCategoriesService;


        public AdsController(IAdsService adService, ISubCategoriesService subCategoriesService)
        {
            this.adService = adService;
            this.subCategoriesService = subCategoriesService;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateAdInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return View(inputModel);
            }

            await adService.CreateAdAsync(inputModel);

            TempData["CreatedAd"] = SuccessfullyCreatedAdMessage;
                

            return RedirectToAction("WaitingForApproval");
        }

        public async Task<IActionResult> GetSubcategoriesAsync(int categoryId)
        {
            var subcategories = await subCategoriesService
                .GetSubcategoriesByCategoryIdAsync(categoryId);

            return Json(subcategories);
        }

        public async Task<IActionResult> AdsByCategory(AdsByCategoryInputModel inputModel)
        {
            var adsByCategoryViewModel = await adService.GetAdsByCategoryViewModelAsync(inputModel.Id, inputModel.PageNumber?? DefaultPageNumber, DefaultPageSize);

             return View(adsByCategoryViewModel);
        }

        public async Task<IActionResult> Details(AdDetailsInputModel inputModel)
        {
            var adDetailsViewModel = await adService.GetAdDetailsViewModelAsync(inputModel.Id);

            return View(adDetailsViewModel);
        }

        [Authorize]
        public async Task<IActionResult> ActiveAds(int? pageNumber)
        {
            var viewModel = await adService.GetMyActiveAdsViewModelsAsync(pageNumber?? DefaultPageNumber, DefaultPageSize);

            return View(viewModel);
        }

        [Authorize]
        public async Task<IActionResult> ArchiveAd(int adId)
        {
            bool isArchived = await adService.ArchiveAdByIdAsync(adId);

            var result = new
            {
                success = true
            };

            return Json(result);
        }

        [Authorize]
        public async Task<IActionResult> ArchivedAds(int? pageNumber)
        {

            var archivedAdsViewModels = await adService.GetArchivedAdsViewModelsAsync(pageNumber ?? DefaultPageNumber, DefaultPageSize);


            return View(archivedAdsViewModels);
        }

        [Authorize]
        public async Task<IActionResult> ActivateAd(int adId)
        {
            bool isActivated = await adService.ActivateAdByIdAsync(adId);

            var result = new
            {
                success = true
            };

            return Json(result);
        }

        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var editAdBindingModel = await adService.GetEditAdBindingModelById(id);

            return View(editAdBindingModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditAdBindingModel bindingModel)
        {
            await adService.EditAd(bindingModel.EditAdInputModel);
            TempData["SuccessfullyEditedAdsMessage"] = SuccessfullyEditedAdsMessage;

            return RedirectToAction("ActiveAds");
        }

        public async Task<IActionResult> Update(UpdateAdInputModel inputModel)
        {
            var isUpdated = await adService.UpdateAdByIdAsync(inputModel.AdId);
            if (!isUpdated)
            {
                TempData["UpdateAdMessage"] = UnSuccessfullyUpdatesAdMessage;
            }
            else
            {
                TempData["UpdateAdMessage"] = SuccessfullyUpdatedAdMessage;
            }

            return RedirectToAction("ActiveAds");
        }

        public async Task<IActionResult> AdsBySubcategory(AdsBySubcategoryInputModel inputModel)
        {
            //TODO: Create input model
            var adsBySubcategoryViewModel = await adService.GetAdsBySubcategoryViewModelAsync(inputModel.SubcategoryId, inputModel.CategoryId, inputModel.PageNumber?? DefaultPageNumber, DefaultPageSize);

            return View(adsBySubcategoryViewModel);
        }

        public async Task<IActionResult> BySearch(AdsBySearchInputModel inputModel)
        {
            var adsBySearchViewModels = await adService.GetAdsBySearchViewModelsAsync(inputModel.Search, DefaultPageNumber, DefaultPageSize);

             return View(adsBySearchViewModels);
        }

        public async Task<IActionResult> AdsByUser(string userId, int? pageNumber)
        {
            var adsByUserBindingModel = await adService.GetAdsByUserBindingModelAsync(userId, pageNumber ?? DefaultPageNumber, DefaultPageSize);

            return View(adsByUserBindingModel);
        }

        public async Task<IActionResult> WaitingForApproval(int? pageNumber)
        {
            var waitingForApprovalAdsByUser = await adService.GetWaitingForApprovalByCurrentUserViewModels(pageNumber ?? DefaultPageNumber, DefaultPageSize);

            return View(waitingForApprovalAdsByUser);
        }

        public async Task<IActionResult> Rejected(int? pageNumber)
        {
            var rejectedAdsByUser = await adService.GetRejectedAdByUserViewModelsAsync(pageNumber ?? DefaultPageNumber, DefaultPageSize);

            return View(rejectedAdsByUser);
        }

        public async Task<IActionResult> SubmitRejectedAd(int rejectionId)
        {
            var isSucceeded = await adService.SubmitRejectedAdAsync(rejectionId);

            return Json(isSucceeded);
        }
    }
}
