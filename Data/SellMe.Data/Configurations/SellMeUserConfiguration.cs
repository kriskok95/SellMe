namespace SellMe.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using SellMe.Data.Models;

    public class SellMeUserConfiguration : IEntityTypeConfiguration<SellMeUser>
    {
        public void Configure(EntityTypeBuilder<SellMeUser> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Address)
                .WithOne(x => x.SellMeUser)
                .HasForeignKey<SellMeUser>(x => x.AddressId);

            builder.HasMany(x => x.SellMeUserFavoriteProducts)
                .WithOne(x => x.SellMeUser)
                .HasForeignKey(x => x.SellMeUserId);
        }
    }
}
