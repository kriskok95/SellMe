using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SellMe.Data.Models;

namespace SellMe.Data.Configurations
{
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
