using AutoMapper;
using eShop.ServicesProduct.API.Models;
using eShop.ServicesProduct.API.Protos.Product;
using eShop.ServicesProduct.API.Repositories.Interfaces;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using static eShop.ServicesProduct.API.Protos.Product.DefaultProductService;

namespace eShop.ServicesProduct.API.Services;

public class ProductService : DefaultProductServiceBase
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly Empty _empty;

    public ProductService(IProductRepository productRepository, IMapper mapper) =>
        (_productRepository, _mapper, _empty) = (productRepository, mapper, new());

    [Authorize]
    public override async Task<ProductDto> AddProduct(ProductDto request, ServerCallContext context)
    {
        var product = _mapper.Map<Product>(request);

        var addedProduct = await _productRepository.AddProductAsync(product);

        return _mapper.Map<ProductDto>(addedProduct);
    }

    [Authorize]
    public override async Task<CountChangeResponse> DeleteBrand(IdRequest request, ServerCallContext context)
    {
        var count = await _productRepository.DeleteBrandAsync(request.Id);

        return new CountChangeResponse { Count = count };
    }

    [Authorize]
    public override async Task<CountChangeResponse> DeleteProduct(IdRequest request, ServerCallContext context)
    {
        var count = await _productRepository.DeleteProductAsync(request.Id);

        return new CountChangeResponse { Count = count };
    }

    [Authorize("Admin")]
    public override async Task<CountChangeResponse> DeleteType(IdRequest request, ServerCallContext context)
    {
        var count = await _productRepository.DeleteProductTypeAsync(request.Id);

        return new CountChangeResponse { Count = count };
    }

    public override async Task GetAllBrand(Empty request, IServerStreamWriter<ProductBrandDto> responseStream, ServerCallContext context)
    {
        var brands = await _productRepository.GetAllBrandsAsync();

        var productBrandDtos = _mapper.Map<IEnumerable<ProductBrandDto>>(brands);

        foreach (var productBrandDto in productBrandDtos)
        {
            await responseStream.WriteAsync(productBrandDto);
        }
    }

    public override async Task GetAllType(Empty request, IServerStreamWriter<ProductTypeDto> responseStream, ServerCallContext context)
    {
        var types = await _productRepository.GetAllProductTypesAsync();

        var productTypeDtos = _mapper.Map<IEnumerable<ProductTypeDto>>(types);

        foreach (var productTypeDto in productTypeDtos)
        {
            await responseStream.WriteAsync(productTypeDto);
        }
    }

    public override async Task<BrandDescriptionResponse> GetBrandDescription(IdRequest request, ServerCallContext context)
    {
        var id = request.Id;

        var description = await _productRepository.GetBrandDescriptionAsync(id);

        return new BrandDescriptionResponse() { Description = description };
    }

    public override async Task<ProductDto?> GetProduct(IdProductRequest request, ServerCallContext context)
    {
        var idProduct = _mapper.Map<IdProduct>(request);

        var productDto = _mapper.Map<ProductDto?>(await _productRepository.GetProductAsync(idProduct));

        return productDto;
    }

    public override async Task GetProductsByFilter(FilterRequest request, IServerStreamWriter<ProductDto> responseStream, ServerCallContext context)
    {
        var searchFilter = _mapper.Map<SearchFilter>(request);

        var products = await _productRepository.GetProductsByFilterAsync(searchFilter);

        foreach (var item in products)
        {
            await responseStream.WriteAsync(_mapper.Map<ProductDto>(item));
        }
    }

    [Authorize]
    public override async Task<CountChangeResponse> UpdateProduct(ProductDto request, ServerCallContext context)
    {
        var product = _mapper.Map<Product>(request);

        var updatedCount = await _productRepository.UpdateProductAsync(product);

        return new CountChangeResponse { Count = updatedCount };
    }

    public override async Task<LengthResponse> GetProductsLengthByFilter(FilterRequest request, ServerCallContext context)
    {
        var searchFilter = _mapper.Map<SearchFilter>(request);

        var length = await _productRepository.GetProductsLengthByFilterAsync(searchFilter);

        return new LengthResponse { Length = length };
    }

    [Authorize("Admin")]
    public override async Task<ProductBrandDto> AddProductBrand(ProductBrandDto request, ServerCallContext context)
    {
        var productBrand = _mapper.Map<ProductBrand>(request);

        var productBrandResponse = await _productRepository.AddProductBrandAsync(productBrand);

        return _mapper.Map<ProductBrandDto>(productBrandResponse);
    }

    [Authorize("Admin")]
    public override async Task<ProductTypeDto> AddProductType(ProductTypeDto request, ServerCallContext context)
    {
        var productType = _mapper.Map<ProductType>(request);

        var productTypeResponse = await _productRepository.AddProductTypeAsync(productType);

        return _mapper.Map<ProductTypeDto>(productTypeResponse);
    }

    public override async Task<ProductsDto> GetProductsByIds(IdProductsRequest request, ServerCallContext context)
    {
        var idProducts = _mapper.Map<IdProducts>(request);

        var products = await _productRepository.GetProductsByIdsAsync(idProducts);

        var productsDto = new ProductsDto();
        var productsDtos = _mapper.Map<IEnumerable<ProductDto>>(products);
        productsDto.Products.AddRange(productsDtos);

        return productsDto;
    }

}