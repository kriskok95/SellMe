using Microsoft.EntityFrameworkCore;

namespace SellMe.Web.Controllers
{
    using SellMe.Services.Interfaces;
    using Microsoft.AspNetCore.Mvc;
    using SellMe.Web.ViewModels.InputModels.Reviews;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;

    public class ReviewsController : Controller
    {
        private const int DefaultPageSize = 10;
        private const int DefaultPageNumber = 1;

        private readonly IReviewsService reviewsService;

        public ReviewsController(IReviewsService reviewsService)
        {
            this.reviewsService = reviewsService;
        }

        [Authorize]
        public async Task<IActionResult> ReviewsByShop(string userId, int? pageNumber)
        {
            var reviewBindingModel = await this.reviewsService.GetReviewsBindingModelByUserId(userId, pageNumber ?? DefaultPageNumber, DefaultPageSize);

            return this.View(reviewBindingModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> LeaveComment(ReviewInputModel inputModel)
        {
            if (this.reviewsService.CheckOwnerIdAndSellerId(inputModel.CreatorId, inputModel.OwnerId))
            {
                ModelState.AddModelError("ShopOwner", "You can't leave a review because you are the owner of the shop!");
            }

            if (!ModelState.IsValid)
            {
                var reviewBindingModel = await this.reviewsService.GetReviewsBindingModelByUserId(inputModel.OwnerId, DefaultPageNumber, DefaultPageSize);
                reviewBindingModel.InputModel = inputModel;
                return this.View("ReviewsByShop", reviewBindingModel);
            }

            await this.reviewsService.CreateReview(inputModel.OwnerId, inputModel.CreatorId, inputModel.Content,
                inputModel.Rating);

            return RedirectToAction("ReviewsByShop", new { userId = inputModel.OwnerId });
        }
    }
}
