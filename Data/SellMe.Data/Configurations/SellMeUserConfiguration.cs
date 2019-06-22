namespace SellMe.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using SellMe.Data.Models;

    public class SellMeUserConfiguration : IEntityTypeConfiguration<SellMeUser>
    {
        public void Configure(EntityTypeBuilder<SellMeUser> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Address)
                .WithOne(x => x.SellMeUser)
                .HasForeignKey<SellMeUser>(x => x.AddressId);

            builder.HasMany(x => x.SellMeUserFavoriteProducts)
                .WithOne(x => x.SellMeUser)
                .HasForeignKey(x => x.SellMeUserId);

            builder.HasMany(x => x.SentBox)
                .WithOne(x => x.Sender)
                .HasForeignKey(x => x.SenderId);

            builder.HasMany(x => x.Inbox)
                .WithOne(x => x.Recipient)
                .HasForeignKey(x => x.RecipientId);

            builder.HasMany(x => x.Orders)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId);
        }
    }
}
