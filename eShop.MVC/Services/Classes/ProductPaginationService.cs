using eShop.MVC.Models.Classes;
using eShop.MVC.Protos.Product;
using eShop.MVC.Services.Intefaces;

namespace eShop.MVC.Services.Classes;

class ProductPaginationService : IProductPaginationService
{
    private readonly IProductService _productService;

    public ProductPaginationService(IProductService productService) =>
        _productService = productService;

    public async Task<PaginationResult<Product>> GetPaginationAsync(FilterRequest filter)
    {
        var productEnumerable = await _productService.GetProductsByFilterAsync(filter);
        var products = productEnumerable.ToList();
        return await GetResultAsync(filter, products);
    }

    private async Task<PaginationResult<Product>> GetResultAsync(FilterRequest filter, IList<Product> products) =>
        new PaginationResult<Product>
        (
            StartPage: filter.Skip,
            EndPage: filter.Skip + products.Count,
            Length: await _productService.GetProductsLengthByFilterAsync(filter),
            Items: products
        );
}
