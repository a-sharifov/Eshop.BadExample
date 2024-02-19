using eShop.ServicesProduct.API.Models;

namespace eShop.ServicesProduct.API.Repositories.Interfaces;

public interface IProductRepository
{
    public Task<Product?> GetProductAsync(IdProduct request);
    public Task<IEnumerable<Product>> GetProductsByFilterAsync(SearchFilter filter);
    public Task<int> GetProductsLengthByFilterAsync(SearchFilter filter);
    public Task<Product> AddProductAsync(Product product);
    public Task<int> UpdateProductAsync(Product product);
    public Task<int> DeleteProductAsync(int id);
    public Task<IEnumerable<ProductBrand>> GetAllBrandsAsync();
    public Task<int> DeleteBrandAsync(int id);
    public Task<string?> GetBrandDescriptionAsync(int id);
    public Task<IEnumerable<ProductType>> GetAllProductTypesAsync();
    public Task<int> DeleteProductTypeAsync(int id);
    public Task<ProductType> AddProductTypeAsync(ProductType productType);
    public Task<ProductBrand> AddProductBrandAsync(ProductBrand productBrand);
    public Task<IEnumerable<Product>> GetProductsByIdsAsync(IdProducts request);
}  