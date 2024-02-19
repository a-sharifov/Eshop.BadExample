using eShop.ServicesProduct.API.Models;
using eShop.ServicesProduct.API.Protos.Product;

namespace eShop.ServicesProduct.API.Repositories.Interfaces;

public interface IProductElasticRepository
{
    public Task AddProductIndexAsync(ProductIndex productIndex);
    public Task DeleteProductIndexAsync(int id);
    public Task<List<int>> FindProductIdListAsync(SearchFilter filter);
    public Task<int> GetLengthAsync(SearchFilter filter);
    public Task UpdateProductIndexAsync(ProductIndex productIndex);
}