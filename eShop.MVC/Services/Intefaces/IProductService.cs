using eShop.MVC.Models.Classes;
using eShop.MVC.Protos.Product;

namespace eShop.MVC.Services.Intefaces;

public interface IProductService
{
    public Task AddProductAsync(Product product, string token);
    public Task<int> DeleteBrandAsync(int id, string token);
    public Task<int> DeleteProductAsync(int id, string token);
    public Task<int> DeleteTypeAsync(int id, string token);
    public Task<IEnumerable<ProductBrand>> GetAllBrandAsync();
    public Task<IEnumerable<ProductType>> GetAllTypeAsync();
    public Task<string> GetBrandDescriptionAsync(int id);
    public Task<Product> GetProductAsync(int id, string? includeTables = null);
    public Task<IEnumerable<Product>> GetProductsByFilterAsync(FilterRequest filter);
    public Task<int> GetProductsLengthByFilterAsync(FilterRequest filter);
    public Task<int> UpdateProductAsync(Product product, string token);
    public Task<IEnumerable<Product>> GetProductsByIdsAsync(IEnumerable<int> ids, string includeFields, string? includeTables = null);
}