using AutoMapper;
using eShop.ServicesCart.API.Models;
using eShop.ServicesCart.API.Protos.Product;
using eShop.ServicesCart.API.Repositories.Interfaces;
using static eShop.ServicesCart.API.Protos.Product.DefaultProductService;

namespace eShop.ServicesCart.API.Repositories.Classes;

public class ProductRepository : IProductRepository
{
    private readonly DefaultProductServiceClient _productService;
    private readonly IMapper _mapper;

    public ProductRepository(DefaultProductServiceClient productService, IMapper mapper) =>
        (_productService, _mapper) = (productService, mapper);

    public async Task<IEnumerable<CartProduct>> GetProductsAsync(IEnumerable<CartProductIndex> cartProducts)
    {
        var request = new IdProductsRequest
        {
            IncludeTables = "ProductSeller,ProductBrand",
            IncludeFields = "Id, Price, Name, Count, ImageUrl, ProductSeller, ProductBrand",
        };

        request.Ids.AddRange(cartProducts.Select(cP => cP.ProductId));

        var productsDto =
            await _productService.GetProductsByIdsAsync(request);

        var products = _mapper.Map<List<CartProduct>>(productsDto.Products);

        products.Select(p =>
            p.Quantity =
                cartProducts.First(cP => cP.ProductId == p.ProductId).Quantity);

        return products;
    }
}
