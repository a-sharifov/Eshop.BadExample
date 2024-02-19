using eShop.ServicesProduct.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;

namespace eShop.ServicesProduct.API.DbContexts.EntityConfigurations;

public class ProductEntityTypeConfiguration
     : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
               .IsRequired()
               .HasMaxLength(64);

        builder.Property(p => p.ImageUrl)
               .IsRequired();

        builder.Property(p => p.Description)
               .HasMaxLength(256);

        builder.HasOne(p => p.ProductType)
               .WithMany()
               .HasForeignKey(p => p.ProductTypeId)
               .OnDelete(DeleteBehavior.Cascade)
               .IsRequired();

        builder.HasOne(p => p.ProductBrand)
               .WithMany()
               .HasForeignKey(p => p.ProductBrandId)
               .OnDelete(DeleteBehavior.Cascade)
               .IsRequired();

        builder.HasOne(p => p.ProductSeller)
               .WithMany()
               .HasForeignKey(p => p.ProductSellerId)
               .OnDelete(DeleteBehavior.Cascade)
               .IsRequired();

        builder.Property(p => p.Composition)
               .HasMaxLength(256);

    }
}
