namespace eShop.ServicesCoupon.API.DbContexts.DbInitializers;

public interface IDbInitializer
{
    public Task InitializeAsync();
}