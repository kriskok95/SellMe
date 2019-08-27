namespace SellMe.Data.Seeding
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Models;

    class PromotionsSeeder : ISeeder
    {
        public async Task SeedAsync(SellMeDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (await dbContext.Promotions.AnyAsync())
            {
                return;
            }

            await dbContext.Promotions.AddAsync(new Promotion
                {Type = "silver", Price = 3.50M, Updates = 3, ActiveDays = 10, CreatedOn = DateTime.UtcNow});
            await dbContext.Promotions.AddAsync(new Promotion
                { Type = "gold", Price = 8.00M, Updates = 10, ActiveDays = 30, CreatedOn = DateTime.UtcNow });
        }
    }
}
