using System;
using System.Linq;
using System.Threading.Tasks;
using SellMe.Data.Models;

namespace SellMe.Data.Seeding
{
    internal class CategoriesSeeder : ISeeder
    {
        public async Task SeedAsync(SellMeDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Categories.Any())
            {
                return;
            }

            await dbContext.Categories.AddAsync(new Category {Name = "Vehicle", FontAwesomeIcon = "fas fa-car-alt", CreatedOn =  DateTime.UtcNow});
        }
    }
}
