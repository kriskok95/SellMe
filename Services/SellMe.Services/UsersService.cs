namespace SellMe.Services
{
    using SellMe.Services.Interfaces;
    using Microsoft.AspNetCore.Http;
    using System.Security.Claims;

    public class UsersService : IUsersService
    {
        private readonly IHttpContextAccessor contextAccessor;

        public UsersService(IHttpContextAccessor contextAccessor)
        {
            this.contextAccessor = contextAccessor;
        }

        public string GetCurrentUserId()
        {
            var currentUserId = this.contextAccessor
                .HttpContext.User
                .FindFirst(ClaimTypes.NameIdentifier)
                .Value;

            return currentUserId;
        }
    }
}
