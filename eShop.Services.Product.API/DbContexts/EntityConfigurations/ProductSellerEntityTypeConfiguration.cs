using eShop.ServicesProduct.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eShop.ServicesProduct.API.DbContexts.EntityConfigurations;

public class ProductSellerEntityTypeConfiguration
    : IEntityTypeConfiguration<ProductSeller>
{
    public void Configure(EntityTypeBuilder<ProductSeller> builder)
    {
        builder.HasKey(pS => pS.Id);

        builder.Property(cT => cT.Seller)
               .IsRequired()
               .HasMaxLength(64);

        builder.HasIndex(cT => cT.Seller)
          .IsUnique();
    }
}
