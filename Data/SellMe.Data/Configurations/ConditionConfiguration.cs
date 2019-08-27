namespace SellMe.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class ConditionConfiguration : IEntityTypeConfiguration<Condition>
    {
        public void Configure(EntityTypeBuilder<Condition> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasMany(x => x.Ads)
                .WithOne(x => x.Condition)
                .HasForeignKey(x => x.ConditionId);
        }
    }
}
