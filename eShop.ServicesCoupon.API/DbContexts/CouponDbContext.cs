using eShop.ServicesCoupon.API.DbContexts.EntityConfigurations;
using eShop.ServicesCoupon.API.Models;
using Microsoft.EntityFrameworkCore;

namespace eShop.ServicesCoupon.API.DbContexts;

public class CouponDbContext : DbContext
{
    public DbSet<Coupon> Coupons { get; set; }

    public CouponDbContext(DbContextOptions<CouponDbContext> options)
       : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfiguration(new CouponEntityTypeConfiguration());
}