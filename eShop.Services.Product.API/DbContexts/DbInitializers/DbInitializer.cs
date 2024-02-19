using eShop.ServicesProduct.API.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace eShop.ServicesProduct.API.DbInitializers;

public class DbInitializer : IDbInitializer
{
    private readonly ProductDbContext _dbContext;

    public DbInitializer(ProductDbContext dbContext) =>
        _dbContext = dbContext;

    public async Task InitializeAsync() =>
            await _dbContext.Database.MigrateAsync();
}