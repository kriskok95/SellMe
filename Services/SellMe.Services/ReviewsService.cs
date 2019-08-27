namespace SellMe.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Castle.Core.Internal;
    using Common;
    using Data;
    using Data.Models;
    using Interfaces;
    using Mapping;
    using Paging;
    using Web.ViewModels.BindingModels.Reviews;
    using Web.ViewModels.ViewModels.Reviews;

    public class ReviewsService : IReviewsService
    {
        private const string CreateReviewArgumentsNotNullOrEmptyErrorMessage = "Some of the arguments are null or empty!";

        private const string OwnerOfTheAdCantLeaveReviewErrorMessage =
            "Seller of the ad can't leave reviews for his ads!";

        private const string ArgumentOutOfRangeErrorMessage = "The rating must be in range between 1 and 5";

        private readonly IUsersService usersService;
        private readonly SellMeDbContext context;

        public ReviewsService(SellMeDbContext context, IUsersService usersService)
        {
            this.usersService = usersService;
            this.context = context;
        }

        public async Task<ReviewsBindingModel> GetReviewsBindingModelByUserId(string userId, int pageNumber, int pageSize)
        {
            if (userId.IsNullOrEmpty())
            {
                throw new ArgumentException(GlobalConstants.InvalidUserIdErrorMessage);
            }

            var owner = await usersService.GetUserByIdAsync(userId);

            var reviewViewModel = GetReviewViewModelsByUserId(userId);
            var paginatedReviewViewModels =
                await PaginatedList<ReviewViewModel>.CreateAsync(reviewViewModel, pageNumber, pageSize);

            var averageRating = context.Reviews.Count(x => x.OwnerId == userId) > 0
                ? context.Reviews.Where(x => x.OwnerId == userId).Average(x => x.Rating)
                : 0;

            var reviewsBindingModel = new ReviewsBindingModel
            {
                OwnerId = userId,
                OwnerUsername = owner.UserName,
                SenderId = usersService.GetCurrentUserId(),
                Votes = GetVotesByStars(userId),
                AverageVote = averageRating,
                ViewModels = paginatedReviewViewModels
            };

            return reviewsBindingModel;
        }

        public async Task CreateReview(string ownerId, string creatorId, string content, int rating)
        {
            if (ownerId.IsNullOrEmpty() || creatorId.IsNullOrEmpty() || content.IsNullOrEmpty())
            {
                throw new ArgumentException(CreateReviewArgumentsNotNullOrEmptyErrorMessage);
            }

            if (rating < 1 || rating > 5)
            {
                throw new ArgumentException(ArgumentOutOfRangeErrorMessage);
            }

            if (ownerId == creatorId)
            {
                throw new InvalidOperationException(OwnerOfTheAdCantLeaveReviewErrorMessage);
            }

            var review = new Review
            {
                OwnerId = ownerId,
                CreatorId = creatorId,
                Comment = content,
                Rating = rating
            };

            await context.Reviews.AddAsync(review);
            await context.SaveChangesAsync();
        }

        public bool CheckOwnerIdAndSellerId(string creatorId, string ownerId)
        {
            return creatorId == ownerId;
        }

        private IQueryable<ReviewViewModel> GetReviewViewModelsByUserId(string userId)
        {
            var reviewViewModels = context.Reviews
                .Where(x => x.OwnerId == userId)
                .To<ReviewViewModel>();

            return reviewViewModels;
        }
        private List<int> GetVotesByStars(string userId)
        {
            List<int> result = new List<int>();
            for (int i = 1; i <= 5; i++)
            {
                result.Add(context.Reviews.Count(x => x.OwnerId == userId && x.Rating == i));
            }

            return result;
        }
    }
}
