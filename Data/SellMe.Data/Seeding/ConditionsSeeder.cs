namespace SellMe.Data.Seeding
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Models;
    using SellMe.Common;

    public class ConditionsSeeder : ISeeder
    {
        public async Task SeedAsync(SellMeDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (await dbContext.Conditions.AnyAsync())
            {
                return;
            }

            await dbContext.AddAsync(new Condition
                {Name = GlobalConstants.ConditionBrandNewName, CreatedOn = DateTime.UtcNow});
            await dbContext.AddAsync(new Condition
                { Name = GlobalConstants.ConditionUsedName, CreatedOn = DateTime.UtcNow });
        }
    }
}
