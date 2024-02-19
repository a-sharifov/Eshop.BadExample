using eShop.ServicesCoupon.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eShop.ServicesCoupon.API.DbContexts.EntityConfigurations;

public class CouponEntityTypeConfiguration : IEntityTypeConfiguration<Coupon>
{
    public void Configure(EntityTypeBuilder<Coupon> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.CouponCode)
            .IsRequired()
            .HasMaxLength(255);

        builder.HasIndex(c => c.CouponCode)
            .IsUnique();

        builder.Property(c => c.Discount)
            .IsRequired();
    }
}
