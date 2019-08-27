namespace SellMe.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasMany(x => x.Ads)
                .WithOne(x => x.Address)
                .HasForeignKey(x => x.AddressId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
