using eShop.ServicesCart.API.Constants;
using eShop.ServicesCart.API.Databases.Configurations;
using eShop.ServicesCart.API.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace eShop.ServicesCart.API.Databases.DbInitializers;

public class DbInitializer : IDbInitializer
{
    public readonly IMongoClient _mongoClient;
    private readonly IMongoCollection<Cart> _cartCollection;

    public DbInitializer(IOptions<DatabaseSettings> options, IMongoClient mongoClient)
    {
        var optionsValue = options.Value;
        _mongoClient = mongoClient;

        _cartCollection = _mongoClient.GetDatabase(optionsValue.DatabaseName)
                                      .GetCollection<Cart>(optionsValue.CollectionName);

    }

    public async Task InitializeAsync() =>
        await CreateIndexAsync();

    private async Task CreateIndexAsync()
    {
        var cartIndexCoursor = await _cartCollection.Indexes.ListAsync();
        var cartIndexCoursorList = await cartIndexCoursor.ToListAsync();

        if (cartIndexCoursorList.Exists(b => b["name"] == CartConstants.UserName))
        {
            return;
        }

        var indexKeysDefinition = Builders<Cart>.IndexKeys
                                                .Ascending(CartConstants.UserName);

        await _cartCollection.Indexes.CreateOneAsync(indexKeysDefinition);
    }
}
