using AutoMapper;
using eShop.ServicesProduct.API.Models;
using eShop.ServicesProduct.API.Repositories.Classes;
using eShop.ServicesProduct.API.Repositories.Interfaces;
using eShop.ServicesProduct.Tests.DbContexts;
using FluentAssertions;
using Moq;
using Xunit;

namespace eShop.ServicesProduct.Tests.Repositories;

public class ProductRepositoryTests
{
    private readonly IMapper _mapper;

    public ProductRepositoryTests()
    {
        // Initialize AutoMapper or use a mock IMapper as needed
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Product, ProductIndex>();
            // Add other mappings here
        });
        _mapper = config.CreateMapper();
    }

    [Fact]
    public async Task GetProductAsync_WhenProductExists_ShouldReturnProduct()
    {
        // Arrange
        using var dbContext = await ProductDbContextBuilder.BuildSeedDbContextAsync();
        var elasticRepositoryMock = new Mock<IProductElasticRepository>();
        var repository = new ProductRepository(dbContext, elasticRepositoryMock.Object, _mapper);

        // Act
        var result = await repository.GetProductAsync(new IdProduct { Id = 1 });

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(1);
    }

    [Fact]
    public async Task GetProductAsync_WhenProductDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        using var dbContext = await ProductDbContextBuilder.BuildSeedDbContextAsync();
        var elasticRepositoryMock = new Mock<IProductElasticRepository>();
        var repository = new ProductRepository(dbContext, elasticRepositoryMock.Object, _mapper);

        // Act
        var result = await repository.GetProductAsync(new IdProduct { Id = 999 });

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetProductsByFilterAsync_WhenProductsExist_ShouldReturnProducts()
    {
        // Arrange
        using var dbContext = await ProductDbContextBuilder.BuildSeedDbContextAsync();
        var elasticRepositoryMock = new Mock<IProductElasticRepository>();
        var repository = new ProductRepository(dbContext, elasticRepositoryMock.Object, _mapper);
        var filter = new SearchFilter { ProductName = "iPhone X" };

        // Act
        var result = await repository.GetProductsByFilterAsync(filter);

        // Assert
        result.Should().NotBeNull();
        result.Should().NotBeEmpty();
        result.Should().AllBeOfType<Product>();
    }

    [Fact]
    public async Task GetProductsByFilterAsync_WhenNoProductsMatchFilter_ShouldReturnEmptyList()
    {
        // Arrange
        using var dbContext = await ProductDbContextBuilder.BuildSeedDbContextAsync();
        var elasticRepositoryMock = new Mock<IProductElasticRepository>();
        var repository = new ProductRepository(dbContext, elasticRepositoryMock.Object, _mapper);
        var filter = new SearchFilter { /* Set filter criteria that won't match any products */ };

        // Act
        var result = await repository.GetProductsByFilterAsync(filter);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public async Task GetProductsLengthByFilterAsync_WhenProductsExist_ShouldReturnLength(int expectedLength)
    {
        // Arrange
        using var dbContext = await ProductDbContextBuilder.BuildSeedDbContextAsync();
        var elasticRepositoryMock = new Mock<IProductElasticRepository>();
        var repository = new ProductRepository(dbContext, elasticRepositoryMock.Object, _mapper);
        var filter = new SearchFilter { /* Set filter criteria to match existing products */ };

        // Act
        var result = await repository.GetProductsLengthByFilterAsync(filter);

        // Assert
        result.Should().Be(expectedLength);
    }

    [Fact]
    public async Task GetProductsLengthByFilterAsync_WhenNoProductsMatchFilter_ShouldReturnZero()
    {
        // Arrange
        using var dbContext = await ProductDbContextBuilder.BuildSeedDbContextAsync();
        var elasticRepositoryMock = new Mock<IProductElasticRepository>();
        var repository = new ProductRepository(dbContext, elasticRepositoryMock.Object, _mapper);
        var filter = new SearchFilter { /* Set filter criteria that won't match any products */ };

        // Act
        var result = await repository.GetProductsLengthByFilterAsync(filter);

        // Assert
        result.Should().Be(0);
    }

    [Fact]
    public async Task AddProductAsync_ShouldAddProductAndReturnIt()
    {
        // Arrange
        using var dbContext = await ProductDbContextBuilder.BuildSeedDbContextAsync();
        var elasticRepositoryMock = new Mock<IProductElasticRepository>();
        var repository = new ProductRepository(dbContext, elasticRepositoryMock.Object, _mapper);
        var newProduct = new Product { /* Set new product data */ };

        // Act
        var result = await repository.AddProductAsync(newProduct);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().BeGreaterThan(0);
    }

    [Fact]
//
    public async Task UpdateProductAsync_ShouldUpdateProductAndReturnModifiedCount()
    {
        // Arrange
        using var dbContext = await ProductDbContextBuilder.BuildSeedDbContextAsync();
        var elasticRepositoryMock = new Mock<IProductElasticRepository>();
        var repository = new ProductRepository(dbContext, elasticRepositoryMock.Object, _mapper);
        var productIdToUpdate = 1; // Set the product ID to update
        var modifiedProduct = new Product
        {

        };

        // Act
        var result = await repository.UpdateProductAsync(modifiedProduct);

        // Assert
        result.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task DeleteProductAsync_ShouldDeleteProductAndReturnDeletedCount()
    {
        // Arrange
        using var dbContext = await ProductDbContextBuilder.BuildSeedDbContextAsync();
        var elasticRepositoryMock = new Mock<IProductElasticRepository>();
        var repository = new ProductRepository(dbContext, elasticRepositoryMock.Object, _mapper);
        var productIdToDelete = 1; // Set the product ID to delete

        // Act
        var result = await repository.DeleteProductAsync(productIdToDelete);

        // Assert
        result.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task GetAllBrandsAsync_ShouldReturnAllProductBrands()
    {
        // Arrange
        using var dbContext = await ProductDbContextBuilder.BuildSeedDbContextAsync();
        var elasticRepositoryMock = new Mock<IProductElasticRepository>();
        var repository = new ProductRepository(dbContext, elasticRepositoryMock.Object, _mapper);

        // Act
        var result = await repository.GetAllBrandsAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().NotBeEmpty();
        result.Should().AllBeOfType<ProductBrand>();
    }

    [Fact]
    public async Task DeleteBrandAsync_ShouldDeleteProductBrandAndReturnDeletedCount()
    {
        // Arrange
        using var dbContext = await ProductDbContextBuilder.BuildSeedDbContextAsync();
        var elasticRepositoryMock = new Mock<IProductElasticRepository>();
        var repository = new ProductRepository(dbContext, elasticRepositoryMock.Object, _mapper);
        var brandIdToDelete = 1; // Set the brand ID to delete

        // Act
        var result = await repository.DeleteBrandAsync(brandIdToDelete);

        // Assert
        result.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task GetBrandDescriptionAsync_ShouldReturnBrandDescription()
    {
        // Arrange
        using var dbContext = await ProductDbContextBuilder.BuildSeedDbContextAsync();
        var elasticRepositoryMock = new Mock<IProductElasticRepository>();
        var repository = new ProductRepository(dbContext, elasticRepositoryMock.Object, _mapper);
        var brandId = 1; // Set the brand ID to retrieve description
        var expectedDescription = "Expected Brand Description"; // Set the expected description

        // Act
        var result = await repository.GetBrandDescriptionAsync(brandId);

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(expectedDescription);
    }

    [Fact]
    public async Task GetAllProductTypesAsync_ShouldReturnAllProductTypes()
    {
        // Arrange
        using var dbContext = await ProductDbContextBuilder.BuildSeedDbContextAsync();
        var elasticRepositoryMock = new Mock<IProductElasticRepository>();
        var repository = new ProductRepository(dbContext, elasticRepositoryMock.Object, _mapper);

        // Act
        var result = await repository.GetAllProductTypesAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().NotBeEmpty();
        result.Should().AllBeOfType<ProductType>();
    }

    [Fact]
    public async Task DeleteProductTypeAsync_ShouldDeleteProductTypeAndReturnDeletedCount()
    {
        // Arrange
        using var dbContext = await ProductDbContextBuilder.BuildSeedDbContextAsync();
        var elasticRepositoryMock = new Mock<IProductElasticRepository>();
        var repository = new ProductRepository(dbContext, elasticRepositoryMock.Object, _mapper);
        var productTypeIdToDelete = 1; // Set the product type ID to delete

        // Act
        var result = await repository.DeleteProductTypeAsync(productTypeIdToDelete);

        // Assert
        result.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task AddProductTypeAsync_ShouldAddProductTypeAndReturnIt()
    {
        // Arrange
        using var dbContext = await ProductDbContextBuilder.BuildSeedDbContextAsync();
        var elasticRepositoryMock = new Mock<IProductElasticRepository>();
        var repository = new ProductRepository(dbContext, elasticRepositoryMock.Object, _mapper);
        var newProductType = new ProductType { /* Set new product type data */ };

        // Act
        var result = await repository.AddProductTypeAsync(newProductType);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().BeGreaterThan(0);
    }
}