using eShop.ServicesProduct.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eShop.ServicesProduct.API.DbContexts.EntityConfigurations;

public class ProductTypeEntityTypeConfiguration
     : IEntityTypeConfiguration<ProductType>
{
    public void Configure(EntityTypeBuilder<ProductType> builder)
    {
        builder.HasKey(pT => pT.Id);

        builder.Property(pT => pT.Type)
               .IsRequired()
               .HasMaxLength(64);

        builder.HasIndex(pT => pT.Type)
          .IsUnique();
    }
}
