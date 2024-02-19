using Microsoft.EntityFrameworkCore;

namespace eShop.ServicesCoupon.API.DbContexts.DbInitializers;

public class DbInitializer : IDbInitializer
{
    private readonly CouponDbContext _dbContext;

    public DbInitializer(CouponDbContext dbContext) =>
        _dbContext = dbContext;

    public async Task InitializeAsync() =>
            await _dbContext.Database.MigrateAsync();
}
