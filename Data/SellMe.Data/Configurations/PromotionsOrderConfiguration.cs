namespace SellMe.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class PromotionsOrderConfiguration : IEntityTypeConfiguration<PromotionOrder>
    {
        public void Configure(EntityTypeBuilder<PromotionOrder> builder)
        {
            builder.HasOne(x => x.Ad)
                .WithMany(x => x.PromotionOrders)
                .HasForeignKey(x => x.AdId);

            builder.HasOne(x => x.Promotion)
                .WithMany(x => x.PromotionOrders)
                .HasForeignKey(x => x.PromotionId);
        }
    }
}
