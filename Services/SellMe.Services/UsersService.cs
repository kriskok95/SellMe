namespace SellMe.Services
{
    using System;
    using System.Linq;

    using SellMe.Services.Interfaces;
    using Microsoft.AspNetCore.Http;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using SellMe.Data.Models;
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore;
    using SellMe.Data;
    using SellMe.Services.Mapping;
    using SellMe.Web.ViewModels.ViewModels.Users;


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
            var currentUserId = this.contextAccessor
                .HttpContext.User
                .FindFirst(ClaimTypes.NameIdentifier)
                .Value;

            return currentUserId;
        }

        public async Task<SellMeUser> GetCurrentUserAsync()
        {
            var currentUser = await this.userManager.GetUserAsync(this.contextAccessor.HttpContext.User);

            return currentUser;
        }

        public async Task<SellMeUser> GetUserByIdAsync(string userId)
        {
            var user = await this.userManager.FindByIdAsync(userId);

            return user;
        }

        public async Task<IEnumerable<UserAllViewModel>> GetAllUserViewModelsAsync()
        {
            var userAllViewModels = await this.context.SellMeUsers
                .Where(x => !x.IsDeleted)
                .To<UserAllViewModel>()
                .ToListAsync();

            return userAllViewModels;
        }

        public async Task<bool> BlockUserByIdAsync(string userId)
        {
            var userFromDb = await this.context.SellMeUsers.FirstOrDefaultAsync(x => x.Id == userId);

            userFromDb.IsDeleted = true;
            userFromDb.DeletedOn = DateTime.UtcNow;

            this.context.SellMeUsers.Update(userFromDb);
            await this.context.SaveChangesAsync();

            await DeleteAdsByUserId(userId);
            
            return true;
        }

        private async Task DeleteAdsByUserId(string userId)
        {
            var ads = this.context
                .Ads
                .Where(x => x.SellerId == userId);

            foreach (var ad in ads)
            {
                ad.IsDeleted = true;
                ad.DeletedOn = DateTime.UtcNow;
            }

            await this.context.SaveChangesAsync();
        }
    }
}
