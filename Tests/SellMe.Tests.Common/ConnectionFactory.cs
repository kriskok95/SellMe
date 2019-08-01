namespace SellMe.Tests
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using SellMe.Data;


    public class ConnectionFactory : IDisposable
    {
        private bool disposedValue = false;

        public SellMeDbContext CreateContextForInMemory()
        {
            var option = new DbContextOptionsBuilder<SellMeDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            var context = new SellMeDbContext(option);
            if (context != null)
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }

            return context;
        }


        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
