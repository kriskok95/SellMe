namespace SellMe.Data.Configurations
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using SellMe.Data.Models;

    public class UpdateAdConfiguration : IEntityTypeConfiguration<UpdateAd>
    {
        public void Configure(EntityTypeBuilder<UpdateAd> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }
}
