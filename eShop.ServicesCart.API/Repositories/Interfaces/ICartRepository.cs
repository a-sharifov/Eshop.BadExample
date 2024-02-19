using eShop.ServicesCart.API.Models;

namespace eShop.ServicesCart.API.Repositories.Interfaces;

public interface ICartRepository
{
    public Task<int> AddCountInCartAsync(string userName, string objectId, int count);
    public Task<bool> AddCouponInCartAsync(string userName, string couponCode);
    public Task<bool> DeleteCouponInCartAsync(string userName);
    public Task<int> AddProductInCartAsync(string userName, CartProductIndex product);
    public Task<int> DeleteProductsInCartAsync(string userName);
    public Task<int> DeleteProductInCartByIndexAsync(string userName, string objectId);
    public Task<Cart> GetCartAsync(string userName);
}