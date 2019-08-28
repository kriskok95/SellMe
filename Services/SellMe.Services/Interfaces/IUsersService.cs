namespace SellMe.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Data.Models;
    using Paging;
    using Web.ViewModels.ViewModels.Users;

    public interface IUsersService
    {
        string GetCurrentUserId();

        Task<SellMeUser> GetCurrentUserAsync();

        Task<SellMeUser> GetUserByIdAsync(string userId);

        Task<PaginatedList<UserAllViewModel>> GetAllUserViewModelsAsync(int pageNumber, int pageSize);

        Task<bool> BlockUserByIdAsync(string userId);

        Task<double> GetRatingByUserAsync(string userId);

        Task<int> GetCountOfAllUsersAsync();

        Task<PaginatedList<BlockedUserAllViewModel>> GetAllBlockedUserViewModels(int pageNumber, int pageSize);

        Task<bool> UnblockUserByIdAsync(string userId);
    }
}
