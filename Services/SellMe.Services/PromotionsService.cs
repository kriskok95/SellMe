namespace SellMe.Services
{
    using SellMe.Common;
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
    using SellMe.Services.Mapping;

    public class PromotionsService : IPromotionsService
    {
        private readonly IAdsService adsService;
        private readonly SellMeDbContext context;

        public PromotionsService(SellMeDbContext context, IAdsService adsService)
        {
            this.adsService = adsService;
            this.context = context;
        }

        public async Task<PromotionBindingModel> GetPromotionBindingModelByAdIdAsync(int adId)
        {
            if (!await this.context.Ads.AnyAsync(x => x.Id == adId))
            {
                throw new ArgumentException(GlobalConstants.InvalidAdIdErrorMessage);
            }

            var adFromDb = await this.adsService.GetAdByIdAsync(adId);

            var promotionViewModels = await this.context
                .Promotions
                .To<PromotionViewModel>()
                .ToListAsync();

            var promotionBindingModel = new PromotionBindingModel
            {
                AdId = adFromDb.Id,
                AdTitle = adFromDb.Title,
                PromotionViewModels = promotionViewModels,
            };

            return promotionBindingModel;
        }

        public async Task CreatePromotionOrderAsync(int adId, int promotionId)
        {
            if (!await this.context.Ads.AnyAsync(x => x.Id == adId))
            {
                throw new ArgumentException(GlobalConstants.InvalidAdIdErrorMessage);
            }

            if (!await this.context.Promotions.AnyAsync(x => x.Id == promotionId))
            {
                throw new ArgumentException(GlobalConstants.InvalidPromotionIdErrorMessage);
            }

            var adFromDb = await this.adsService.GetAdByIdAsync(adId);
            var promotionFromDb = await this.GetPromotionByIdAsync(promotionId);

            var promotionOrder = new PromotionOrder
            {
                AdId = adId,
                PromotionId = promotionId,
                CreatedOn = DateTime.UtcNow,
                ActiveTo = DateTime.UtcNow.AddDays(promotionFromDb.ActiveDays),
                Price = promotionFromDb.Price,
            };

            adFromDb.Updates += promotionFromDb.Updates;

            await this.context.PromotionOrders.AddAsync(promotionOrder);
            await this.context.SaveChangesAsync();
        }

        private async Task<Promotion> GetPromotionByIdAsync(int promotionId)
        {
            var promotion = await this.context
                .Promotions
                .FirstOrDefaultAsync(x => x.Id == promotionId);

            return promotion;
        }
    }
}
