using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SellMe.Data.Models;

namespace SellMe.Data.Configurations
{
    public class PromotionConfiguration : IEntityTypeConfiguration<Promotion>
    {
        public void Configure(EntityTypeBuilder<Promotion> builder)
        {
            builder.HasMany(x => x.Ads)
                .WithOne(x => x.Promotion)
                .HasForeignKey(x => x.PromotionId);
        }
    }
}
