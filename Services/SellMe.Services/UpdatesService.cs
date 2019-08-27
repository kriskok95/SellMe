namespace SellMe.Services
{
    using System;
    using System.Threading.Tasks;
    using Common;
    using Data;
    using Data.Models;
    using Interfaces;
    using Microsoft.EntityFrameworkCore;

    public class UpdatesService : IUpdatesService
    {
        private readonly SellMeDbContext context;

        public UpdatesService(SellMeDbContext context)
        {
            this.context = context;
        }

        public async Task CreateUpdateAdAsync(int adId)
        {
            if(!await context.Ads.AnyAsync(x => x.Id == adId))
            {
                throw new ArgumentException(GlobalConstants.InvalidAdIdErrorMessage);
            }

            var updateAd = new UpdateAd
            {
                AdId = adId
            };
            await context.UpdateAds.AddAsync(updateAd);
            await context.SaveChangesAsync();
        }
    }
}
