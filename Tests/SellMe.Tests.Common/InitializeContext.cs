namespace SellMe.Tests.Common
{
    using System;
    using Data;
    using Microsoft.EntityFrameworkCore;

    public static class InitializeContext
    {
        public static SellMeDbContext CreateContextForInMemory()
        {
            var option = new DbContextOptionsBuilder<SellMeDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            var context = new SellMeDbContext(option);
            if (context != null)
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }

            return context;
        }
    }
}
