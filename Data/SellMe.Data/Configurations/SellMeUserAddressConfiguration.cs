namespace SellMe.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    class SellMeUserAddressConfiguration : IEntityTypeConfiguration<SellMeUserAddress>
    {
        public void Configure(EntityTypeBuilder<SellMeUserAddress> builder)
        {
            builder.HasKey(x => new {x.AddressId, x.SellMeUserId});

            builder.HasOne(x => x.SellMeUser)
                .WithMany(x => x.SellMeUserAddresses)
                .HasForeignKey(x => x.SellMeUserId);

            builder.HasOne(x => x.Address)
                .WithMany(x => x.SellMeUserAddresses)
                .HasForeignKey(x => x.AddressId);
        }
    }
}
