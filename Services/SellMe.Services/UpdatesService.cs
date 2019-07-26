using System.Threading.Tasks;
using SellMe.Data;
using SellMe.Data.Models;

namespace SellMe.Services
{
    using SellMe.Services.Interfaces;

    public class UpdatesService : IUpdatesService
    {
        private readonly SellMeDbContext context;

        public UpdatesService(SellMeDbContext context)
        {
            this.context = context;
        }

        public async Task CreateUpdateAd(int adId)
        {
            var updateAd = new UpdateAd
            {
                AdId = adId
            };
            await this.context.UpdateAds.AddAsync(updateAd);
            await this.context.SaveChangesAsync();
        }
    }
}
