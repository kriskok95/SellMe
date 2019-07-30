using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SellMe.Services.Mapping;
using SellMe.Web.ViewModels.ViewModels.Reviews;

namespace SellMe.Services
{
    using System.Threading.Tasks;
    using SellMe.Web.ViewModels.BindingModels.Reviews;
    using SellMe.Services.Interfaces;
    using SellMe.Data;
    using SellMe.Data.Models;

    public class ReviewsService : IReviewsService
    {
        private readonly IUsersService usersService;
        private readonly SellMeDbContext context;

        public ReviewsService(IUsersService usersService, SellMeDbContext context)
        {
            this.usersService = usersService;
            this.context = context;
        }

        public async Task<ReviewsBindingModel> GetReviewsBindingModelByUserId(string userId)
        {
            var owner = await this.usersService.GetUserById(userId);

            var reviewsBindingModel = new ReviewsBindingModel
            {
                OwnerId = userId,
                OwnerUsername = owner.UserName,
                SenderId = this.usersService.GetCurrentUserId(),
                ViewModels = await this.GetReviewViewModelsByUserId(userId),
            };

            return reviewsBindingModel;
        }

        private async Task<IEnumerable<ReviewViewModel>> GetReviewViewModelsByUserId(string userId)
        {
            var reviewViewModels = await this.context.Reviews
                .Where(x => x.OwnerId == userId)
                .To<ReviewViewModel>()
                .ToListAsync();

            return reviewViewModels;
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
    }
}
