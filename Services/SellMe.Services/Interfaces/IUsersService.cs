namespace SellMe.Services.Interfaces
{
    using System.Threading.Tasks;

    using SellMe.Data.Models;
    using System.Collections.Generic;
    using SellMe.Web.ViewModels.ViewModels.Users;

    public interface IUsersService
    {
        string GetCurrentUserId();

        Task<SellMeUser> GetCurrentUserAsync();

        Task<SellMeUser> GetUserByIdAsync(string userId);

        Task<IEnumerable<UserAllViewModel>> GetAllUserViewModelsAsync();

        Task<bool> BlockUserByIdAsync(string userId);
    }
}
