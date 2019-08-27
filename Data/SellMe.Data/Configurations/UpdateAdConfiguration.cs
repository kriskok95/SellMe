namespace SellMe.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class UpdateAdConfiguration : IEntityTypeConfiguration<UpdateAd>
    {
        public void Configure(EntityTypeBuilder<UpdateAd> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }
}
