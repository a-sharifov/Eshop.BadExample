using eShop.ServicesCart.API.Extensions;
using eShop.ServicesCart.API.Models;
using eShop.ServicesCart.API.Repositories.Interfaces;
using StackExchange.Redis;

namespace eShop.ServicesCart.API.Repositories.Classes;

public class CartRedisRepository : ICartRedisRepository
{
    private readonly IDatabase _database;

    public CartRedisRepository(IConnectionMultiplexer connectionMultiplexer) =>
        _database = connectionMultiplexer.GetDatabase();

    public async Task<bool> SetCartAsync(Cart cart, uint expireMinutes = 0) =>
        await _database.StringSetAsync(cart.UserName, cart.ToJsonSerailize(),
            expireMinutes == 0 ? null : TimeSpan.FromMinutes(expireMinutes));

    public async Task<Cart?> GetCartAsync(string userName, uint expireMinutes = 0)
    {
        var serializerCart = await _database.StringGetAsync(userName);
        if (serializerCart.IsNullOrEmpty)
        {
            return null;
        }
        var cart = serializerCart.ToString().ToJsonDeserialize<Cart>();
        await SetCartAsync(cart, expireMinutes);
        return cart;
    }

    public async Task<bool> DeleteCartAsync(string userName) =>
        await _database.KeyDeleteAsync(userName);
}