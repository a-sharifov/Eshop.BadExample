using eShop.ServicesProduct.API.DbContexts.EntityConfigurations;
using eShop.ServicesProduct.API.Models;
using Microsoft.EntityFrameworkCore;

namespace eShop.ServicesProduct.API.DbContexts;

public class ProductDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductSeller> productSellers { get; set; }
    public DbSet<ProductType> ProductTypes { get; set; }
    public DbSet<ProductBrand> ProductBrands { get; set; }

    public ProductDbContext(DbContextOptions<ProductDbContext> options)
        : base(options) { }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new ProductEntityTypeConfiguration());
        builder.ApplyConfiguration(new ProductSellerEntityTypeConfiguration());
        builder.ApplyConfiguration(new ProductTypeEntityTypeConfiguration());
        builder.ApplyConfiguration(new ProductBrandEntityTypeConfiguration());
    }
}