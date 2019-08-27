namespace SellMe.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class AdRejectionConfiguration : IEntityTypeConfiguration<AdRejection>
    {
        public void Configure(EntityTypeBuilder<AdRejection> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }
}
