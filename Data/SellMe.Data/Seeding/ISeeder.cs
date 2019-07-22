namespace SellMe.Data.Seeding
{
    using System;
    using System.Threading.Tasks;

    public interface ISeeder
    {
        Task SeedAsync(SellMeDbContext dbContext, IServiceProvider serviceProvider);
    }
}
