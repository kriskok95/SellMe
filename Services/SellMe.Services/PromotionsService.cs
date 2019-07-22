namespace SellMe.Services
{
    using SellMe.Services.Interfaces;
    using System.Threading.Tasks;
    using AutoMapper;
    using SellMe.Web.ViewModels.BindingModels.Promotions;
    using SellMe.Web.ViewModels.ViewModels.Promotions;
    using System;
    using SellMe.Data.Models;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using SellMe.Data;

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

        public async Task CreatePromotionForAdAsync(int adId, string promotionType)
        {
            bool isThereAnyActivePromotion = await this.context
                .Promotions
                .AnyAsync(x => x.AdId == adId && x.IsActive);

            DateTime activeFrom = DateTime.UtcNow;

            if (isThereAnyActivePromotion)
            {
                activeFrom = (this.context.Promotions
                    .Where(x => x.IsActive && x.AdId == adId)
                    .OrderByDescending(x => x.ActiveTo)
                    .FirstOrDefault()?.ActiveTo ?? DateTime.UtcNow);
            }

            if (promotionType == "silver")
            { 
                var promotion = new Promotion
                {
                    AdId = adId,
                    Updates = SilverAdUpdates,
                    ActiveTo = activeFrom.AddDays(SilverAdActiveDays),
                    Type = "Silver",
                };
                await this.context.Promotions.AddAsync(promotion);
                await this.context.SaveChangesAsync();
            }
            else if (promotionType == "gold")
            {
                var promotion = new Promotion
                {
                    AdId = adId,
                    Updates = GoldAdUpdates,
                    ActiveTo = activeFrom.AddDays(GoldAdActiveDays),
                    Type = "Gold",
                };
                await this.context.Promotions.AddAsync(promotion);
                await this.context.SaveChangesAsync();
            }
            
        }
    }
}
