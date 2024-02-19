using eShop.ServicesProduct.API.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace eShop.ServicesProduct.Tests.DbContexts;

internal class ProductDbContextBuilder
{
    private static readonly Random rand = new();

    public static async Task<ProductDbContext> BuildSeedDbContextAsync()
    {
        var options = new DbContextOptionsBuilder<ProductDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var dbContext = new ProductDbContext(options);
        await dbContext.Database.EnsureCreatedAsync();
        await InitializeAsync(dbContext);

        return dbContext;
    }

    private async static Task InitializeAsync(ProductDbContext dbContext)
    {
        await dbContext.ProductTypes.AddRangeAsync(ProductDbContextSeed.GetDefaultsProductTypes());
        await dbContext.ProductBrands.AddRangeAsync(ProductDbContextSeed.GetDefaultsProductBrands());
        await dbContext.productSellers.AddRangeAsync(ProductDbContextSeed.GetDefaultsProductSellers());

        await dbContext.SaveChangesAsync();

        await dbContext.Products.AddRangeAsync(ProductDbContextSeed.GetDefaultsProducts().ToArray());

        await dbContext.SaveChangesAsync();
    }

}