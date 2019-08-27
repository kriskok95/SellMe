namespace SellMe.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Data;
    using Data.Models;
    using Interfaces;
    using Mapping;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Web.ViewModels.ViewModels.Users;

    public class UsersService : IUsersService
    {
        private readonly IHttpContextAccessor contextAccessor;
        private readonly UserManager<SellMeUser> userManager;
        private readonly SellMeDbContext context;

        public UsersService(IHttpContextAccessor contextAccessor, UserManager<SellMeUser> userManager, SellMeDbContext context)
        {
            this.contextAccessor = contextAccessor;
            this.userManager = userManager;
            this.context = context;
        }

        public string GetCurrentUserId()
        {
            var currentUserId = contextAccessor
                .HttpContext.User
                .FindFirst(ClaimTypes.NameIdentifier)
                .Value;

            return currentUserId;
        }

        public async Task<SellMeUser> GetCurrentUserAsync()
        {
            var currentUser = await userManager.GetUserAsync(contextAccessor.HttpContext.User);

            return currentUser;
        }

        public async Task<SellMeUser> GetUserByIdAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);

            return user;
        }

        public async Task<IEnumerable<UserAllViewModel>> GetAllUserViewModelsAsync()
        {
            var userAllViewModels = await context.SellMeUsers
                .Where(x => !x.IsDeleted)
                .To<UserAllViewModel>()
                .ToListAsync();

            return userAllViewModels;
        }

        public async Task<bool> BlockUserByIdAsync(string userId)
        {
            var userFromDb = await context.SellMeUsers.FirstOrDefaultAsync(x => x.Id == userId);

            userFromDb.IsDeleted = true;
            userFromDb.DeletedOn = DateTime.UtcNow;

            context.SellMeUsers.Update(userFromDb);
            await context.SaveChangesAsync();

            await DeleteAdsByUserId(userId);
            
            return true;
        }

        public async Task<double> GetRatingByUser(string sellerId)
        {
            var user = await GetUserByIdAsync(sellerId);

            var rating = user.OwnedReviews.Any() ? user.OwnedReviews.Average(x => x.Rating) : 0;

            return rating;
        }

        public async Task<int> GetCountOfAllUsersAsync()
        {
            var allUsersCount = await context.Users.CountAsync(x => !x.IsDeleted);

            return allUsersCount;
        }

        private async Task DeleteAdsByUserId(string userId)
        {
            var ads = context
                .Ads
                .Where(x => x.SellerId == userId);

            foreach (var ad in ads)
            {
                ad.IsDeleted = true;
                ad.DeletedOn = DateTime.UtcNow;
            }

            await context.SaveChangesAsync();
        }
    }
}
