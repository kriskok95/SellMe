namespace SellMe.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Data.Models;
    using Web.ViewModels.ViewModels.Users;

    public interface IUsersService
    {
        string GetCurrentUserId();

        Task<SellMeUser> GetCurrentUserAsync();

        Task<SellMeUser> GetUserByIdAsync(string userId);

        Task<IEnumerable<UserAllViewModel>> GetAllUserViewModelsAsync();

        Task<bool> BlockUserByIdAsync(string userId);

        Task<double> GetRatingByUser(string userId);

        Task<int> GetCountOfAllUsersAsync();
    }
}
