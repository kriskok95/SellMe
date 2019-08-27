namespace SellMe.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class SellMeUserConfiguration : IEntityTypeConfiguration<SellMeUser>
    {
        public void Configure(EntityTypeBuilder<SellMeUser> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasMany(x => x.SellMeUserFavoriteProducts)
                .WithOne(x => x.SellMeUser)
                .HasForeignKey(x => x.SellMeUserId);

            builder.HasMany(x => x.SentBox)
                .WithOne(x => x.Sender)
                .HasForeignKey(x => x.SenderId);

            builder.HasMany(x => x.Inbox)
                .WithOne(x => x.Recipient)
                .HasForeignKey(x => x.RecipientId);

            builder.HasMany(x => x.Ads)
                .WithOne(x => x.Seller)
                .HasForeignKey(x => x.SellerId);

            builder.HasMany(x => x.OwnedReviews)
                .WithOne(x => x.Owner)
                .HasForeignKey(x => x.OwnerId);

            builder.HasMany(x => x.CreatedReviews)
                .WithOne(x => x.Creator)
                .HasForeignKey(x => x.CreatorId);
            
        }
    }
}
