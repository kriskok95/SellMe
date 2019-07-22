namespace SellMe.Services.Interfaces
{
    using System.Threading.Tasks;
    using SellMe.Web.ViewModels.BindingModels.Promotions;

    public interface IPromotionsService
    {
        Task<PromotionBindingModel> GetPromotionBindingModelByAdIdAsync(int adId);

        Task CreatePromotionForAdAsync(int adId, string promotionType);
    }
}
