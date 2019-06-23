namespace SellMe.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using SellMe.Data.Models;

    public class SellMeDbContext : IdentityDbContext<SellMeUser>
    {
        public SellMeDbContext(DbContextOptions<SellMeDbContext> options)
            : base(options)
        {
        }

        public DbSet<SellMeUser> SellMeUsers { get; set; }

        public DbSet<Address> Addresses { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Image> Images { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Review> Reviews { get; set; }

        public DbSet<SellMeUserFavoriteProduct> SellMeUserFavoriteProducts { get; set; }

        public DbSet<SubCategory> SubCategories { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Condition> Conditions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(SellMeDbContext).Assembly);
        }
    }
}
