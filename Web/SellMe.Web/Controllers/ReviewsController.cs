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
            var reviewBindingModel = await this.reviewsService.GetReviewsBindingModelByUserId(userId, pageNumber?? DefaultPageNumber, DefaultPageSize);

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
