using AutoMapper;
using eShop.MVC.Exstensions;
using eShop.MVC.Models.Classes;
using eShop.MVC.Protos.Product;
using eShop.MVC.Services.Intefaces;
using Grpc.Core;
using static eShop.MVC.Protos.Product.DefaultProductService;

namespace eShop.MVC.Services.Classes;

public class ProductService : IProductService
{
    private readonly DefaultProductServiceClient _serviceClient;
    private readonly IMapper _mapper;
    private readonly Empty _empty;

    public ProductService(DefaultProductServiceClient serviceClient, IMapper mapper) =>
        (_serviceClient, _mapper, _empty) = (serviceClient, mapper, new());

    public async Task AddProductAsync(Product product, string token)
    {
        var headers = CreateAuthorizationBearerMetadata(token);

        await _serviceClient.AddProductAsync(_mapper.Map<ProductDto>(product), headers);
    }

    public async Task<int> DeleteBrandAsync(int id, string token)
    {
        var headers = CreateAuthorizationBearerMetadata(token);

        var countResponse = await _serviceClient.DeleteBrandAsync(new IdRequest { Id = id }, headers);
        return countResponse.Count;
    }

    public async Task<int> DeleteProductAsync(int id, string token)
    {
        var headers = CreateAuthorizationBearerMetadata(token);

        var countResponse = await _serviceClient.DeleteProductAsync(new IdRequest { Id = id }, headers);
        return countResponse.Count;
    }

    public async Task<int> DeleteTypeAsync(int id, string token)
    {
        var headers = CreateAuthorizationBearerMetadata(token);

        var countResponse = await _serviceClient.DeleteTypeAsync(new IdRequest { Id = id }, headers);
        return countResponse.Count;
    }

    public async Task<IEnumerable<ProductBrand>> GetAllBrandAsync()
    {
        using var call = _serviceClient.GetAllBrand(_empty);
        var brands = new List<ProductBrand>();

        while (await call.ResponseStream.MoveNext())
        {
            brands.Add(_mapper.Map<ProductBrand>(call.ResponseStream.Current));
        }

        return brands;
    }

    public async Task<IEnumerable<ProductType>> GetAllTypeAsync()
    {
        using var call = _serviceClient.GetAllType(_empty);

        var types = new List<ProductType>();

        while (await call.ResponseStream.MoveNext())
        {
            types.Add(_mapper.Map<ProductType>(call.ResponseStream.Current));
        }

        return types;
    }

    public async Task<string> GetBrandDescriptionAsync(int id)
    {
        var descriptionResponse = await _serviceClient.GetBrandDescriptionAsync(new IdRequest { Id = id });
        return descriptionResponse.Description;
    }

    public async Task<Product> GetProductAsync(int id, string? includeTables = null)
    {
        var productDto = await _serviceClient.GetProductAsync(new IdProductRequest { Id = id, IncludeTables = includeTables });
        return _mapper.Map<Product>(productDto);
    }

    public async Task<IEnumerable<Product>> GetProductsByFilterAsync(FilterRequest filter)
    {
        using var call = _serviceClient.GetProductsByFilter(filter);

        var products = new List<Product>();

        while (await call.ResponseStream.MoveNext())
        {
            products.Add(_mapper.Map<Product>(call.ResponseStream.Current));
        }

        return products;
    }

    public async Task<IEnumerable<Product>> GetProductsByIds(List<int> ids, string includeTables, string includeFields)
    {
        var idProductsRequest = new IdProductsRequest
            {
                IncludeFields = includeFields,
                IncludeTables = includeTables,
            };
        idProductsRequest.Ids.AddRange(ids);

        return _mapper.Map<IEnumerable<Product>>(await _serviceClient.GetProductsByIdsAsync(idProductsRequest));
    }

    public async Task<int> UpdateProductAsync(Product product, string token)
    {
        var headers = CreateAuthorizationBearerMetadata(token);

        var countResponse = await _serviceClient.UpdateProductAsync(_mapper.Map<ProductDto>(product), headers);
        return countResponse.Count;
    }

    public async Task<int> GetProductsLengthByFilterAsync(FilterRequest filter)
    {
        var lengthResponse = await _serviceClient.GetProductsLengthByFilterAsync(filter);
        return lengthResponse.Length;
    }

    public async Task<IEnumerable<Product>> GetProductsByIdsAsync(IEnumerable<int> ids, string includeFields, string? includeTables = null)
    {
        var request = new IdProductsRequest
        {
            IncludeFields = includeFields,
            IncludeTables = includeTables
        };

        request.Ids.AddRange(ids);

        var productsDto = await _serviceClient.GetProductsByIdsAsync(request);
        return _mapper.Map<IEnumerable<Product>>(productsDto.Products);
    }

    private Metadata CreateAuthorizationBearerMetadata(string token) =>
        MetadataExtension.CreateAuthorizationBearerMetadata(token);
}