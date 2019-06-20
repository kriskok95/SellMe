using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SellMe.Data.Models;

namespace SellMe.Data.Configurations
{
    public class SellMeUserFavoriteProductConfiguration : IEntityTypeConfiguration<SellMeUserFavoriteProduct>
    {
        public void Configure(EntityTypeBuilder<SellMeUserFavoriteProduct> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }
}
