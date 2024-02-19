using eShop.ServicesCart.API.Models;

namespace eShop.ServicesCart.API.Repositories.Interfaces;

public interface IProductRepository
{
    public Task<IEnumerable<CartProduct>> GetProductsAsync(IEnumerable<CartProductIndex> cartProducts);
}