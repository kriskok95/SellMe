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
    using SellMe.Services.Mapping;

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








            //bool isThereAnyActivePromotion = await this.context
            //    .Promotions
            //    .AnyAsync(x => x.AdId == adId && x.IsActive);

            //DateTime activeFrom = DateTime.UtcNow;

            //if (isThereAnyActivePromotion)
            //{
            //    activeFrom = (this.context.Promotions
            //        .Where(x => x.IsActive && x.AdId == adId)
            //        .OrderByDescending(x => x.ActiveTo)
            //        .FirstOrDefault()?.ActiveTo ?? DateTime.UtcNow);
            //}

            //if (promotionType == "silver")
            //{
            //    var promotion = new Promotion
            //    {
            //        AdId = adId,
            //        Updates = SilverAdUpdates,
            //        ActiveTo = activeFrom.AddDays(SilverAdActiveDays),
            //        Type = "Silver",
            //    };
            //    await this.context.Promotions.AddAsync(promotion);
            //    await this.context.SaveChangesAsync();
            //}
            //else if (promotionType == "gold")
            //{
            //    var promotion = new Promotion
            //    {
            //        AdId = adId,
            //        Updates = GoldAdUpdates,
            //        ActiveTo = activeFrom.AddDays(GoldAdActiveDays),
            //        Type = "Gold",
            //    };
            //    await this.context.Promotions.AddAsync(promotion);
            //    await this.context.SaveChangesAsync();
            //}

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
