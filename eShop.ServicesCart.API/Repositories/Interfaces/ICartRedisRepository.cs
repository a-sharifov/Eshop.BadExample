using eShop.ServicesCart.API.Models;
using MongoDB.Bson;

namespace eShop.ServicesCart.API.Repositories.Interfaces;

public interface ICartRedisRepository
{
    public Task<Cart?> GetCartAsync(string userName, uint expireMinutes = 0);
    public Task<bool> SetCartAsync(Cart cart, uint expireMinutes = 0);
    public Task<bool> DeleteCartAsync(string userName);
}