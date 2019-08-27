namespace SellMe.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class SellMeUserFavoriteProductConfiguration : IEntityTypeConfiguration<SellMeUserFavoriteProduct>
    {
        public void Configure(EntityTypeBuilder<SellMeUserFavoriteProduct> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }
}
