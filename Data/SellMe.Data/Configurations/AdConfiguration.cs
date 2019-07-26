namespace SellMe.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using SellMe.Data.Models;

    public class AdConfiguration : IEntityTypeConfiguration<Ad>
    {
        public void Configure(EntityTypeBuilder<Ad> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasMany(x => x.SellMeUserFavoriteProducts)
                .WithOne(x => x.Ad)
                .HasForeignKey(x => x.AdId);

            builder.HasMany(x => x.Reviews)
                .WithOne(x => x.Ad)
                .HasForeignKey(x => x.AdId);

            builder.HasMany(x => x.Images)
                .WithOne(x => x.Ad)
                .HasForeignKey(x => x.AdId);

            builder.HasMany(x => x.Messages)
                .WithOne(x => x.Ad)
                .HasForeignKey(x => x.AdId);

            builder.HasMany(x => x.AdViews)
                .WithOne(x => x.Ad)
                .HasForeignKey(x => x.AdId);

            builder.HasMany(x => x.UpdateAds)
                .WithOne(x => x.Ad)
                .HasForeignKey(x => x.AdId);

            builder.Property(p => p.Price)
                .HasColumnType("decimal(18,2)");
        }
    }
}
