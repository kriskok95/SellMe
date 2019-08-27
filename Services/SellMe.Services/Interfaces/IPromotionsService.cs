namespace SellMe.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Web.Infrastructure.Models;
    using Web.ViewModels.BindingModels.Promotions;

    public interface IPromotionsService
    {
        Task<PromotionBindingModel> GetPromotionBindingModelByAdIdAsync(int adId);

        Task CreatePromotionOrderAsync(int adId, int promotionId);

        Task<List<int>> GetTheCountOfPromotionsForTheLastTenDaysAsync();
    }
}
