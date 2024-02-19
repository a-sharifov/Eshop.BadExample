namespace eShop.Services.Identity.API.DbContexts.DbInitializers;

public interface IDbInitializer
{
    public Task InitializeAsync();
}