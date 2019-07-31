namespace SellMe.Web.Controllers
{
    using SellMe.Services.Interfaces;
    using Microsoft.AspNetCore.Mvc;
    using SellMe.Web.ViewModels.InputModels.Reviews;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;

    public class ReviewsController : Controller
    {
        private readonly IReviewsService reviewsService;

        public ReviewsController(IReviewsService reviewsService)
        {
            this.reviewsService = reviewsService;
        }

        [Authorize]
        public async Task<IActionResult> ReviewsByShop(string userId)
        {
            var reviewBindingModel = await this.reviewsService.GetReviewsBindingModelByUserId(userId);

            return this.View(reviewBindingModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> LeaveComment(ReviewInputModel inputModel)
        {
            await this.reviewsService.CreateReview(inputModel.OwnerId, inputModel.CreatorId, inputModel.Content,
                inputModel.Rating);

            return RedirectToAction("ReviewsByShop", new{userId = inputModel.OwnerId});
        }
    }
}
