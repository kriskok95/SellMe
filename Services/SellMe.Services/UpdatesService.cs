namespace SellMe.Services
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;

    using SellMe.Common;
    using SellMe.Data;
    using SellMe.Data.Models;
    using SellMe.Services.Interfaces;

    public class UpdatesService : IUpdatesService
    {
        private readonly SellMeDbContext context;

        public UpdatesService(SellMeDbContext context)
        {
            this.context = context;
        }

        public async Task CreateUpdateAdAsync(int adId)
        {
            if(!await this.context.Ads.AnyAsync(x => x.Id == adId))
            {
                throw new ArgumentException(GlobalConstants.InvalidAdIdErrorMessage);
            }

            var updateAd = new UpdateAd
            {
                AdId = adId
            };
            await this.context.UpdateAds.AddAsync(updateAd);
            await this.context.SaveChangesAsync();
        }
    }
}
