using eShop.MVC.Models.Classes;

namespace eShop.MVC.Services.Intefaces;

public interface ICartService
{
    public Task<int> AddCountInCartAsync(string userName, string objectId, int count, string token);
    public Task<int> AddProductInCartAsync(string userName, int productId, int quantity, string token);
    public Task<int> DeleteProductInCartByIndexAsync(string userName, string objectId, string token);
    public Task<Cart> GetCartAsync(string userName, string token);
    Task<int> DeleteProductsAsync(string userName, string token);
}