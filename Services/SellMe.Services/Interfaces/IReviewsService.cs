namespace SellMe.Services.Interfaces
{
    using System.Threading.Tasks;
    using SellMe.Web.ViewModels.BindingModels.Reviews;
    public interface IReviewsService
    {
        Task<ReviewsBindingModel> GetReviewsBindingModelByUserId(string userId, int pageNumber, int pageSize);

        Task CreateReview(string ownerId, string creatorId, string content, int rating);

        bool CheckOwnerIdAndSellerId(string creatorId, string ownerId);
    }
}
