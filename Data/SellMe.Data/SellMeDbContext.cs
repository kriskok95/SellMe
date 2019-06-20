using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SellMe.Data.Models;

namespace SellMe.Data
{
    public class SellMeDbContext : IdentityDbContext<SellMeUser>
    {
        public SellMeDbContext(DbContextOptions<SellMeDbContext> options)
            : base(options)
        {
        }

        public DbSet<SellMeUser> SellMeUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
