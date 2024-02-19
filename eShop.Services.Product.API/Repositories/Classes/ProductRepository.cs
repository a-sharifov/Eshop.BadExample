using AutoMapper;
using eShop.ServicesProduct.API.DbContexts;
using eShop.ServicesProduct.API.Extensions;
using eShop.ServicesProduct.API.Models;
using eShop.ServicesProduct.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Nest;
using System.Linq.Dynamic.Core;
    
namespace eShop.ServicesProduct.API.Repositories.Classes;

public class ProductRepository : IProductRepository
{

    private readonly ProductDbContext _dbContext;
    private readonly IProductElasticRepository _elasticRepository;
    private readonly IMapper _mapper;

    public ProductRepository(ProductDbContext dbContext, IProductElasticRepository elasticRepository, IMapper mapper) =>
        (_dbContext, _elasticRepository, _mapper) = (dbContext, elasticRepository, mapper);


    public async Task<Product?> GetProductAsync(IdProduct request)
    {
        var product = await _dbContext.Products.AsNoTracking()
                                               .DynamicInclude(request.IncludeTables)
                                               .FirstOrDefaultAsync(p => p.Id == request.Id);

        return product;
    }

    public async Task<IEnumerable<Product>> GetProductsByFilterAsync(SearchFilter filter)
    {
        var productIdList = await _elasticRepository.FindProductIdListAsync(filter);

        return await GetProductsByIdsAsync(productIdList, filter.IncludeTables, filter.IncludeFields);
    }

    private async Task<IEnumerable<Product>> GetProductsByIdsAsync
        (IList<int> productIdList, string? includeTables, string? includeFields)
    {
        var productDtos = new List<Product>(productIdList.Count);
        var query = _dbContext.Products.AsNoTracking();

        foreach (var id in productIdList)
        {
            var product = await GetProductByIdAsync(id, includeTables, includeFields, query);
            productDtos.AddIfNotNull(product);
        }

        return productDtos;
    }

    private async Task<Product?> GetProductByIdAsync
        (int id, string? includeTables, string? includeFields, IQueryable<Product> query) =>
            await query.Where(p => p.Id == id)
                       .DynamicInclude(includeTables)
                       .Select<Product>($"new Product({includeFields})")
                       .FirstOrDefaultAsync();


    public async Task<int> GetProductsLengthByFilterAsync(SearchFilter filter) =>
           await _elasticRepository.GetLengthAsync(filter);

    public async Task<Product> AddProductAsync(Product product)
    {
        await AddPropertiesIfExistsAsync(product);
        var productResponse = await _dbContext.Products.AddAsync(product);
        await _dbContext.SaveChangesAsync();
        await _elasticRepository.AddProductIndexAsync(_mapper.Map<ProductIndex>(productResponse.Entity));
        return productResponse.Entity;
    }

    public async Task<int> UpdateProductAsync(Product product)
    {
        product.ProductType = await GetAndOrSaveProductTypeAsync(product.ProductType);
        product.ProductBrand = await GetAndOrSaveProductBrandAsync(product.ProductBrand);
        await AddPropertiesIfExistsAsync(product);
        product.ProductBrandId = product.ProductBrand.Id;
        product.ProductTypeId = product.ProductType.Id;
        var seller = _dbContext.productSellers.Where(s => s.Seller == product.ProductSeller.Seller).FirstOrDefault();
        product.ProductSeller = seller;
        product.ProductSellerId = seller.Id;

        _dbContext.Entry(product).State = EntityState.Modified;
        return await _dbContext.SaveChangesAsync();
    }

    private async Task AddPropertiesIfExistsAsync(Product product)
    {
        var productBrand = await _dbContext.ProductBrands
          .FirstOrDefaultAsync(pB => pB.Brand == product.ProductBrand.Brand);
        if (productBrand != null)
        {
            product.ProductBrandId = productBrand.Id;
            product.ProductBrand = productBrand;
        }

        var productType = await _dbContext.ProductTypes
            .FirstOrDefaultAsync(pT => pT.Type == product.ProductType.Type);
        if (productType != null)
        {
            product.ProductTypeId = productType.Id;
            product.ProductType = productType;
        }

        var productSeller = await _dbContext.productSellers
            .FirstOrDefaultAsync(pS => pS.Seller == product.ProductSeller.Seller);
        if (productSeller != null)
        {
            product.ProductSellerId = productSeller.Id;
            product.ProductSeller = productSeller;
        }
    }

    private async Task<ProductBrand> GetAndOrSaveProductBrandAsync(ProductBrand productBrand)
    {
        var firstProductBrand = await _dbContext.ProductBrands.FirstOrDefaultAsync(pB => pB.Brand == productBrand.Brand);
        if (firstProductBrand == null)
        {
            var productBrandEntity = await _dbContext.ProductBrands.AddAsync(productBrand);
            firstProductBrand = productBrandEntity.Entity;
            await _dbContext.SaveChangesAsync();
        }
        return firstProductBrand;
    }

    private async Task<ProductType> GetAndOrSaveProductTypeAsync(ProductType productType)
    {
        var firstProductType = await _dbContext.ProductTypes.FirstOrDefaultAsync(pT => pT.Type == productType.Type);
        if (firstProductType == null)
        {
            var productTypeEntity = await _dbContext.ProductTypes.AddAsync(productType);
            firstProductType = productTypeEntity.Entity;
            await _dbContext.SaveChangesAsync();
        }
        return firstProductType;
    }

    public async Task<int> DeleteProductAsync(int id)
    {
        await _elasticRepository.DeleteProductIndexAsync(id);

        return await _dbContext.Products.AsNoTracking()
                                        .Where(p => p.Id == id)
                                        .ExecuteDeleteAsync();
    }

    public async Task<IEnumerable<ProductBrand>> GetAllBrandsAsync() =>
        await _dbContext.ProductBrands.AsNoTracking()
                                      .Select(pB => pB)
                                      .ToListAsync();

    public async Task<int> DeleteBrandAsync(int id) =>
        await _dbContext.ProductBrands.AsNoTracking()
                                      .Select(pB => pB.Id == id)
                                      .ExecuteDeleteAsync();

    public async Task<string?> GetBrandDescriptionAsync(int id) =>
            await _dbContext.Products.AsNoTracking()
                                     .Where(pB => pB.Id == id)
                                     .Select(pB => pB.Description)
                                     .FirstOrDefaultAsync();

    public async Task<IEnumerable<ProductType>> GetAllProductTypesAsync() =>
        await _dbContext.ProductTypes.AsNoTracking()
                                     .Select(pT => pT)
                                     .ToListAsync();

    public async Task<int> DeleteProductTypeAsync(int id) =>
        await _dbContext.ProductTypes.AsNoTracking()
                                     .Select(pT => pT.Id == id)
                                     .ExecuteDeleteAsync();

    public async Task<ProductType> AddProductTypeAsync(ProductType productType)
    {
        var productTypeResponse = await _dbContext.ProductTypes.AddAsync(productType);
        await _dbContext.SaveChangesAsync();
        return productTypeResponse.Entity;
    }

    public async Task<ProductBrand> AddProductBrandAsync(ProductBrand productBrand)
    {
        var productBrandResponse = await _dbContext.ProductBrands.AddAsync(productBrand);
        await _dbContext.SaveChangesAsync();
        return productBrandResponse.Entity;
    }

    public async Task<IEnumerable<Product>> GetProductsByIdsAsync(IdProducts request) =>
            await GetProductsByIdsAsync(request.Ids, request.IncludeTables, request.IncludeFields);
}


//public class ProductRepository : IProductRepository
//{
//    private readonly ProductDbContext _dbContext;
//    private readonly IProductElasticRepository _elasticRepository;
//    private readonly IMapper _mapper;
//    private readonly ILogger _logger;

//    public ProductRepository(
//        ProductDbContext dbContext,
//        IProductElasticRepository elasticRepository,
//        IMapper mapper,
//        ILogger logger)
//    {
//        _dbContext = dbContext;
//        _elasticRepository = elasticRepository;
//        _mapper = mapper;
//        _logger = logger;
//    }

//    public async Task<ProductDto?> GetProductAsync(IdProductRequest request)
//    {
//        try
//        {
//            _logger.Information("GetProductAsync called with ID: {ProductId}", request.Id);

//            var product = await _dbContext.Products.AsNoTracking()
//                .DynamicInclude(request.IncludeTables)
//                .FirstOrDefaultAsync(p => p.Id == request.Id);

//            if (product != null)
//            {
//                _logger.Information("Product found with ID: {ProductId}", product.Id);
//            }
//            else
//            {
//                _logger.Information("Product not found with ID: {ProductId}", request.Id);
//            }

//            return _mapper.Map<ProductDto>(product);
//        }
//        catch (Exception ex)
//        {
//            _logger.Error(ex, "Error in GetProductAsync");
//            throw;
//        }
//    }

//    public async Task<IEnumerable<ProductDto>> GetProductsByFilterAsync(FilterRequest filter)
//    {
//        try
//        {
//            _logger.Information("GetProductsByFilterAsync called with filter: {@Filter}", filter);

//            var productIdList = await _elasticRepository.FindProductIdListAsync(filter);

//            if (productIdList == null)
//            {
//                _logger.Information("No product IDs found in the filter.");
//                return new List<ProductDto>();
//            }

//            return await GetProductsByIdsAsync(productIdList, filter.IncludeTables, filter.IncludeFields);
//        }
//        catch (Exception ex)
//        {
//            _logger.Error(ex, "Error in GetProductsByFilterAsync");
//            throw;
//        }
//    }

//    public async Task<IEnumerable<ProductDto>> GetProductsByIdsAsync(IEnumerable<int> productIdList, string includeTables, string includeFields)
//    {
//        try
//        {
//            _logger.Information("GetProductsByIdsAsync called with Product IDs: {ProductIds}", string.Join(", ", productIdList));

//            var productDtos = new List<ProductDto>();
//            var query = _dbContext.Products.AsNoTracking();

//            foreach (var id in productIdList)
//            {
//                var product = await query.Where(p => p.Id == id)
//                    .DynamicInclude(includeTables)
//                    .Select<Product>($"new Product({includeFields})")
//                    .FirstOrDefaultAsync();

//                productDtos.Add(_mapper.Map<ProductDto>(product));
//            }

//            return productDtos;
//        }
//        catch (Exception ex)
//        {
//            _logger.Error(ex, "Error in GetProductsByIdsAsync");
//            throw;
//        }
//    }

//    public async Task<int> GetProductsLengthByFilterAsync(FilterRequest filter)
//    {
//        try
//        {
//            _logger.Information("GetProductsLengthByFilterAsync called with filter: {@Filter}", filter);

//            var length = await _elasticRepository.GetLengthAsync(filter);

//            _logger.Information("Products length found: {Length}", length);

//            return length;
//        }
//        catch (Exception ex)
//        {
//            _logger.Error(ex, "Error in GetProductsLengthByFilterAsync");
//            throw;
//        }
//    }

//    public async Task<ProductDto> AddProductAsync(ProductDto productDto)
//    {
//        try
//        {
//            _logger.Information("AddProductAsync called with ProductDto: {@ProductDto}", productDto);

//            var product = _mapper.Map<Product>(productDto);
//            await AddPropertiesIfExistsAsync(product);
//            var productResponse = await _dbContext.Products.AddAsync(product);
//            await _dbContext.SaveChangesAsync();
//            await _elasticRepository.AddProductIndexAsync(_mapper.Map<ProductIndex>(productResponse.Entity));

//            _logger.Information("Product added with ID: {ProductId}", productResponse.Entity.Id);

//            return _mapper.Map<ProductDto>(productResponse.Entity);
//        }
//        catch (Exception ex)
//        {
//            _logger.Error(ex, "Error in AddProductAsync");
//            throw;
//        }
//    }

//    public async Task<int> UpdateProductAsync(ProductDto productDto)
//    {
//        try
//        {
//            _logger.Information("UpdateProductAsync called with ProductDto: {@ProductDto}", productDto);

//            var product = _mapper.Map<Product>(productDto);

//            product.ProductType = await GetAndOrSaveProductTypeAsync(productDto.ProductTypeDto);
//            product.ProductBrand = await GetAndOrSaveProductBrandAsync(productDto.ProductBrandDto);
//            product.ProductBrandId = product.ProductBrand.Id;
//            product.ProductTypeId = product.ProductType.Id;
//            var seller = _dbContext.productSellers.Where(s => s.Seller == product.ProductSeller.Seller).FirstOrDefault();
//            product.ProductSeller = seller;
//            product.ProductSellerId = seller.Id;

//            _dbContext.Entry(product).State = EntityState.Modified;
//            var updatedCount = await _dbContext.SaveChangesAsync();

//            _logger.Information("Product updated with ID: {ProductId}", product.Id);

//            return updatedCount;
//        }
//        catch (Exception ex)
//        {
//            _logger.Error(ex, "Error in UpdateProductAsync");
//            throw;
//        }
//    }

//    public async Task<int> DeleteProductAsync(int id)
//    {
//        try
//        {
//            _logger.Information("DeleteProductAsync called with ID: {ProductId}", id);

//            await _elasticRepository.DeleteProductIndexAsync(id);

//            var deletedCount = await _dbContext.Products.AsNoTracking()
//                .Where(p => p.Id == id)
//                .ExecuteDeleteAsync();

//            _logger.Information("Product deleted with ID: {ProductId}", id);

//            return deletedCount;
//        }
//        catch (Exception ex)
//        {
//            _logger.Error(ex, "Error in DeleteProductAsync");
//            throw;
//        }
//    }

//    public async Task<IEnumerable<ProductBrandDto>> GetAllBrandsAsync()
//    {
//        try
//        {
//            _logger.Information("GetAllBrandsAsync called");

//            var brands = await _dbContext.ProductBrands.AsNoTracking()
//                .Select(pB => _mapper.Map<ProductBrandDto>(pB))
//                .ToListAsync();

//            _logger.Information("Retrieved {BrandCount} brands", brands.Count);

//            return brands;
//        }
//        catch (Exception ex)
//        {
//            _logger.Error(ex, "Error in GetAllBrandsAsync");
//            throw;
//        }
//    }

//    public async Task<int> DeleteBrandAsync(int id)
//    {
//        try
//        {
//            _logger.Information("DeleteBrandAsync called with ID: {BrandId}", id);

//            var deletedCount = await _dbContext.ProductBrands.AsNoTracking()
//                .Where(pB => pB.Id == id)
//                .ExecuteDeleteAsync();

//            _logger.Information("Brand deleted with ID: {BrandId}", id);

//            return deletedCount;
//        }
//        catch (Exception ex)
//        {
//            _logger.Error(ex, "Error in DeleteBrandAsync");
//            throw;
//        }
//    }

//    public async Task<BrandDescriptionResponse> GetBrandDescriptionAsync(int id)
//    {
//        try
//        {
//            _logger.Information("GetBrandDescriptionAsync called with ID: {BrandId}", id);

//            var description = await _dbContext.Products.AsNoTracking()
//                .Where(pB => pB.Id == id)
//                .Select(pB => pB.Description)
//                .FirstOrDefaultAsync();

//            _logger.Information("Brand description retrieved for ID: {BrandId}", id);

//            return new BrandDescriptionResponse { Description = description };
//        }
//        catch (Exception ex)
//        {
//            _logger.Error(ex, "Error in GetBrandDescriptionAsync");
//            throw;
//        }
//    }

//    public async Task<IEnumerable<ProductTypeDto>> GetAllProductTypesAsync()
//    {
//        try
//        {
//            _logger.Information("GetAllProductTypesAsync called");

//            var productTypes = await _dbContext.ProductTypes.AsNoTracking()
//                .Select(pT => _mapper.Map<ProductTypeDto>(pT))
//                .ToListAsync();

//            _logger.Information("Retrieved {ProductTypeCount} product types", productTypes.Count);

//            return productTypes;
//        }
//        catch (Exception ex)
//        {
//            _logger.Error(ex, "Error in GetAllProductTypesAsync");
//            throw;
//        }
//    }

//    public async Task<int> DeleteProductTypeAsync(int id)
//    {
//        try
//        {
//            _logger.Information("DeleteProductTypeAsync called with ID: {ProductTypeId}", id);

//            var deletedCount = await _dbContext.ProductTypes.AsNoTracking()
//                .Where(pT => pT.Id == id)
//                .ExecuteDeleteAsync();

//            _logger.Information("Product type deleted with ID: {ProductTypeId}", id);

//            return deletedCount;
//        }
//        catch (Exception ex)
//        {
//            _logger.Error(ex, "Error in DeleteProductTypeAsync");
//            throw;
//        }
//    }

//    public async Task<ProductTypeDto> AddProductTypeAsync(ProductTypeDto productTypeDto)
//    {
//        try
//        {
//            _logger.Information("AddProductTypeAsync called with ProductTypeDto: {@ProductTypeDto}", productTypeDto);

//            var productType = _mapper.Map<ProductType>(productTypeDto);
//            var productTypeResponse = await _dbContext.ProductTypes.AddAsync(productType);
//            await _dbContext.SaveChangesAsync();

//            _logger.Information("Product type added with ID: {ProductTypeId}", productTypeResponse.Entity.Id);

//            return _mapper.Map<ProductTypeDto>(productTypeResponse.Entity);
//        }
//        catch (Exception ex)
//        {
//            _logger.Error(ex, "Error in AddProductTypeAsync");
//            throw;
//        }
//    }

//    public async Task<ProductBrandDto> AddProductBrandAsync(ProductBrandDto productBrandDto)
//    {
//        try
//        {
//            _logger.Information("AddProductBrandAsync called with ProductBrandDto: {@ProductBrandDto}", productBrandDto);

//            var productBrand = _mapper.Map<ProductBrand>(productBrandDto);
//            var productBrandResponse = await _dbContext.ProductBrands.AddAsync(productBrand);
//            await _dbContext.SaveChangesAsync();

//            _logger.Information("Product brand added with ID: {ProductBrandId}", productBrandResponse.Entity.Id);

//            return _mapper.Map<ProductBrandDto>(productBrandResponse.Entity);
//        }
//        catch (Exception ex)
//        {
//            _logger.Error(ex, "Error in AddProductBrandAsync");
//            throw;
//        }
//    }

//    public async Task<ProductsDto> GetProductsByIdsAsync(IdProductsRequest request)
//    {
//        try
//        {
//            _logger.Information("GetProductsByIdsAsync called with IDs: {ProductIds}", string.Join(", ", request.Ids));

//            var products = await GetProductsByIdsAsync(request.Ids, request.IncludeTables, request.IncludeFields);
//            var productsDto = new ProductsDto();
//            productsDto.Products.AddRange(products);

//            _logger.Information("Retrieved {ProductCount} products by IDs", productsDto.Products.Count);

//            return productsDto;
//        }
//        catch (Exception ex)
//        {
//            _logger.Error(ex, "Error in GetProductsByIdsAsync");
//            throw;
//        }
//    }

//    private async Task AddPropertiesIfExistsAsync(Product product)
//    {
//        var productBrand = await _dbContext.ProductBrands
//          .FirstOrDefaultAsync(pB => pB.Brand == product.ProductBrand.Brand);
//        if (productBrand != null)
//        {
//            product.ProductBrandId = productBrand.Id;
//            product.ProductBrand = productBrand;
//        }

//        var productType = await _dbContext.ProductTypes
//            .FirstOrDefaultAsync(pT => pT.Type == product.ProductType.Type);
//        if (productType != null)
//        {
//            product.ProductTypeId = productType.Id;
//            product.ProductType = productType;
//        }

//        var productSeller = await _dbContext.productSellers
//            .FirstOrDefaultAsync(pS => pS.Seller == product.ProductSeller.Seller);
//        if (productSeller != null)
//        {
//            product.ProductSellerId = productSeller.Id;
//            product.ProductSeller = productSeller;
//        }
//    }

//    private async Task<ProductBrand> GetAndOrSaveProductBrandAsync(ProductBrandDto productBrand)
//    {
//        var firstProductBrand = await _dbContext.ProductBrands.FirstOrDefaultAsync(pB => pB.Brand == productBrand.Brand);
//        if (firstProductBrand == null)
//        {
//            var productBrandEntity = await _dbContext.ProductBrands.AddAsync(_mapper.Map<ProductBrand>(productBrand));
//            firstProductBrand = productBrandEntity.Entity;
//            await _dbContext.SaveChangesAsync();
//        }
//        return firstProductBrand;
//    }

//    private async Task<ProductType> GetAndOrSaveProductTypeAsync(ProductTypeDto productType)
//    {
//        var firstProductType = await _dbContext.ProductTypes.FirstOrDefaultAsync(pT => pT.Type == productType.Type);
//        if (firstProductType == null)
//        {
//            var productTypeEntity = await _dbContext.ProductTypes.AddAsync(_mapper.Map<ProductType>(productType));
//            firstProductType = productTypeEntity.Entity;
//            await _dbContext.SaveChangesAsync();
//        }
//        return firstProductType;
//    }
//}