namespace SellMe.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    /// <summary>
    /// Configuration for category entity.
    /// </summary>
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasMany(x => x.SubCategories)
                .WithOne(x => x.Category)
                .HasForeignKey(x => x.CategoryId);

            builder.HasMany(x => x.Ads)
                .WithOne(x => x.Category)
                .HasForeignKey(x => x.CategoryId);
        }
    }
}
