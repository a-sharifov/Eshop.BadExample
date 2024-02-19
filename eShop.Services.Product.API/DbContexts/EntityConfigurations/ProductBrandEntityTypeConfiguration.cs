using eShop.ServicesProduct.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eShop.ServicesProduct.API.DbContexts.EntityConfigurations;

public class ProductBrandEntityTypeConfiguration
     : IEntityTypeConfiguration<ProductBrand>
{
    public void Configure(EntityTypeBuilder<ProductBrand> builder)
    {
        builder.HasKey(pB => pB.Id);

        builder.Property(pB => pB.Description)
               .HasMaxLength(256);

        builder.Property(pB => pB.Brand)
               .IsRequired()
               .HasMaxLength(64);

        builder.HasIndex(pB => pB.Brand)
               .IsUnique();
    }
}