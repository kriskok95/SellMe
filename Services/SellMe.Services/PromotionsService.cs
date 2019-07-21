using SellMe.Data;

namespace SellMe.Services
{
    using SellMe.Services.Interfaces;
    using System.Threading.Tasks;
    using AutoMapper;
    using SellMe.Web.ViewModels.BindingModels.Promotions;
    using SellMe.Web.ViewModels.ViewModels.Promotions;
    using System;
    using SellMe.Data.Models;

    public class PromotionsService : IPromotionsService
    {
        private const int SilverAdUpdates = 5;
        private const int GoldAdUpdates = 10;
        private const int SilverAdActiveDays = 10;
        private const int GoldAdActiveDays = 30;


        private readonly IAdsService adsService;
        private readonly IMapper mapper;
        private readonly SellMeDbContext context;

        public PromotionsService(IAdsService adsService, IMapper mapper, SellMeDbContext context)
        {
            this.adsService = adsService;
            this.mapper = mapper;
            this.context = context;
        }

        public async Task<PromotionBindingModel> GetPromotionBindingModelByAdIdAsync(int adId)
        {
            var adFromDb = await this.adsService.GetAdByIdAsync(adId);

            var promotionViewModel = this.mapper.Map<PromotionViewModel>(adFromDb);
            var promotionBindingModel = new PromotionBindingModel
            {
                PromotionViewModel = promotionViewModel
            };

            return promotionBindingModel;
        }

        public void CreatePromotionForAdAsync(int adId, string promotionType)
        {
            //TODO: Refactor this method.
            if (promotionType == "silver")
            {
                var promotion = new Promotion
                {
                    AdId = adId,
                    Updates = SilverAdUpdates,
                    ActiveTo = DateTime.UtcNow.AddDays(SilverAdActiveDays),
                    Type = "Silver",
                };
                this.context.Promotions.Add(promotion);
                this.context.SaveChanges();
            }
            else if (promotionType == "gold")
            {
                var promotion = new Promotion
                {
                    AdId = adId,
                    Updates = GoldAdUpdates,
                    ActiveTo = DateTime.UtcNow.AddDays(GoldAdActiveDays),
                    Type = "Gold",
                };
                this.context.Promotions.Add(promotion);
                this.context.SaveChanges();
            }
            
        }
    }
}
