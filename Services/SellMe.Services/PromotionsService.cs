namespace SellMe.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Common;
    using Data;
    using Data.Models;
    using Interfaces;
    using Mapping;
    using Microsoft.EntityFrameworkCore;
    using Web.ViewModels.BindingModels.Promotions;
    using Web.ViewModels.ViewModels.Promotions;

    public class PromotionsService : IPromotionsService
    {
        private const int PromotionsBoughtCount = 10;

        private readonly IAdsService adsService;
        private readonly SellMeDbContext context;

        public PromotionsService(SellMeDbContext context, IAdsService adsService)
        {
            this.adsService = adsService;
            this.context = context;
        }

        public async Task<PromotionBindingModel> GetPromotionBindingModelByAdIdAsync(int adId)
        {
            if (!await context.Ads.AnyAsync(x => x.Id == adId))
            {
                throw new ArgumentException(GlobalConstants.InvalidAdIdErrorMessage);
            }

            var adFromDb = await adsService.GetAdByIdAsync(adId);

            var promotionViewModels = await context
                .Promotions
                .To<PromotionViewModel>()
                .ToListAsync();

            var promotionBindingModel = new PromotionBindingModel
            {
                AdId = adFromDb.Id,
                AdTitle = adFromDb.Title,
                PromotionViewModels = promotionViewModels
            };

            return promotionBindingModel;
        }

        public async Task CreatePromotionOrderAsync(int adId, int promotionId)
        {
            if (!await context.Ads.AnyAsync(x => x.Id == adId))
            {
                throw new ArgumentException(GlobalConstants.InvalidAdIdErrorMessage);
            }

            if (!await context.Promotions.AnyAsync(x => x.Id == promotionId))
            {
                throw new ArgumentException(GlobalConstants.InvalidPromotionIdErrorMessage);
            }

            var adFromDb = await adsService.GetAdByIdAsync(adId);
            var promotionFromDb = await GetPromotionByIdAsync(promotionId);

            var promotionOrder = new PromotionOrder
            {
                AdId = adId,
                PromotionId = promotionId,
                CreatedOn = DateTime.UtcNow,
                ActiveTo = DateTime.UtcNow.AddDays(promotionFromDb.ActiveDays),
                Price = promotionFromDb.Price
            };

            adFromDb.Updates += promotionFromDb.Updates;

            await context.PromotionOrders.AddAsync(promotionOrder);
            await context.SaveChangesAsync();
        }

        public async Task<List<int>> GetTheCountOfPromotionsForTheLastTenDaysAsync()
        {
            var promotionsCount = new List<int>();

            for (DateTime i = DateTime.UtcNow.AddDays(-GlobalConstants.PromotionsBoughtStatisticDaysCount + 1); i < DateTime.UtcNow; i = i.AddDays(1))
            {
                var currentDayPromotionsCount = await this.context
                    .PromotionOrders
                    .CountAsync(x => x.CreatedOn.DayOfYear == i.DayOfYear);
                promotionsCount.Add(currentDayPromotionsCount);
            }

            return promotionsCount;
        }

        private async Task<Promotion> GetPromotionByIdAsync(int promotionId)
        {
            var promotion = await context
                .Promotions
                .FirstOrDefaultAsync(x => x.Id == promotionId);

            return promotion;
        }
    }
}
