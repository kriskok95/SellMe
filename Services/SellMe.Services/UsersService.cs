namespace SellMe.Services
{
    using SellMe.Services.Interfaces;
    using Microsoft.AspNetCore.Http;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using SellMe.Data.Models;

    public class UsersService : IUsersService
    {
        private readonly IHttpContextAccessor contextAccessor;
        private readonly UserManager<SellMeUser> userManager;

        public UsersService(IHttpContextAccessor contextAccessor, UserManager<SellMeUser> userManager)
        {
            this.contextAccessor = contextAccessor;
            this.userManager = userManager;
        }

        public string GetCurrentUserId()
        {
            var currentUserId = this.contextAccessor
                .HttpContext.User
                .FindFirst(ClaimTypes.NameIdentifier)
                .Value;

            return currentUserId;
        }

        public SellMeUser GetCurrentUser()
        {
            var currentUser = this.userManager.GetUserAsync(this.contextAccessor.HttpContext.User)
                .GetAwaiter()
                .GetResult();

            return currentUser;
        }

        public async Task<SellMeUser> GetUserByIdAsync(string userId)
        {
            var user = await this.userManager.FindByIdAsync(userId);

            return user;
        }
    }
}
