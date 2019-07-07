using SellMe.Data.Models;

namespace SellMe.Services.Interfaces
{
    public interface IUsersService
    {
        string GetCurrentUserId();

        SellMeUser GetCurrentUser();
    }
}
