namespace SellMe.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class SubCategoryConfiguration : IEntityTypeConfiguration<SubCategory>
    {
        public void Configure(EntityTypeBuilder<SubCategory> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasMany(x => x.Ads)
                .WithOne(x => x.SubCategory)
                .HasForeignKey(x => x.SubCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
