namespace SellMe.Services.Interfaces
{
    using System.Threading.Tasks;
    using SellMe.Data.Models;

    public interface IUsersService
    {
        string GetCurrentUserId();

        SellMeUser GetCurrentUser();

        Task<SellMeUser> GetUserByIdAsync(string userId);
    }
}
