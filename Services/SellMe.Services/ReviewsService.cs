namespace SellMe.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using SellMe.Web.ViewModels.BindingModels.Reviews;
    using SellMe.Services.Interfaces;
    using SellMe.Data;
    using SellMe.Data.Models;
    using System;
    using System.Linq;
    using SellMe.Services.Mapping;
    using SellMe.Services.Paging;
    using SellMe.Web.ViewModels.ViewModels.Reviews;

    public class ReviewsService : IReviewsService
    {
        private readonly IUsersService usersService;
        private readonly SellMeDbContext context;

        public ReviewsService(SellMeDbContext context, IUsersService usersService)
        {
            this.usersService = usersService;
            this.context = context;
        }

        public async Task<ReviewsBindingModel> GetReviewsBindingModelByUserId(string userId, int pageNumber, int pageSize)
        {
            var owner = await this.usersService.GetUserByIdAsync(userId);

            var reviewViewModel = this.GetReviewViewModelsByUserId(userId);
            var paginatedReviewViewModels =
                await PaginatedList<ReviewViewModel>.CreateAsync(reviewViewModel, pageNumber, pageSize);

            var averageRating = this.context.Reviews.Count(x => x.OwnerId == userId) > 0
                ? this.context.Reviews.Where(x => x.OwnerId == userId).Average(x => x.Rating)
                : 0;

            var reviewsBindingModel = new ReviewsBindingModel
            {
                OwnerId = userId,
                OwnerUsername = owner.UserName,
                SenderId = this.usersService.GetCurrentUserId(),
                Votes = this.GetVotesByStars(userId),
                AverageVote = averageRating,
                ViewModels = paginatedReviewViewModels,
            };

            return reviewsBindingModel;
        }

        public async Task CreateReview(string ownerId, string creatorId, string content, int rating)
        {
            if (ownerId == creatorId)
            {
                throw new InvalidOperationException("Seller of the ad can't leave reviews for his ads!");
            }

            var review = new Review
            {
                OwnerId = ownerId,
                CreatorId = creatorId,
                Comment = content,
                Rating = rating,
            };

            await this.context.Reviews.AddAsync(review);
            await this.context.SaveChangesAsync();
        }

        public bool CheckOwnerIdAndSellerId(string creatorId, string ownerId)
        {
            return creatorId == ownerId;
        }

        private IQueryable<ReviewViewModel> GetReviewViewModelsByUserId(string userId)
        {
            var reviewViewModels = this.context.Reviews
                .Where(x => x.OwnerId == userId)
                .To<ReviewViewModel>();

            return reviewViewModels;
        }
        private List<int> GetVotesByStars(string userId)
        {
            List<int> result = new List<int>();
            for (int i = 1; i <= 5; i++)
            {
                result.Add(this.context.Reviews.Count(x => x.OwnerId == userId && x.Rating == i));
            }

            return result;
        }
    }
}
