using eShop.ServicesProduct.API.Models;

namespace eShop.ServicesProduct.Tests.DbContexts;

public class ProductDbContextSeed
{
    private static readonly Random rand = new();

    public static IEnumerable<ProductType> GetDefaultsProductTypes() =>
        new ProductType[]
        {
            new(){Type = "Watch"},
            new(){Type = "Smarphone"},
            new(){ Type = "Drink" },
            new(){ Type = "Food" },
            new(){ Type = "Beauty" },
            new(){ Type = "Home" },
            new(){ Type = "Car" },
            new(){ Type = "Pc" },
            new(){ Type = "Console" }
        };

    public static IEnumerable<ProductBrand> GetDefaultsProductBrands() =>
        new ProductBrand[]
        {
            new(){Brand = "Rolex"},
            new(){Brand = "Apple"},
            new(){Brand = "Coca Cola"},
            new(){Brand = "Pepsi"},
            new(){Brand = "Lays"},
            new(){Brand = "Loreal"},
            new(){Brand = "Samsung"},
            new(){Brand = "BMW"},
            new(){Brand = "Audi"},
            new(){Brand = "Dell"},
            new(){Brand = "HP"},
            new(){Brand = "Sony"},
        };

    public static IEnumerable<ProductSeller> GetDefaultsProductSellers() =>
        new ProductSeller[]
        {
            new(){Seller = "Alice"},
            new(){Seller = "Bob"},
            new(){Seller = "Charlie"},
            new(){Seller = "David"},
            new(){Seller = "Eva"},
            new(){Seller = "Frank"},
            new(){Seller = "Grace"},
            new(){Seller = "Henry"},
            new(){Seller = "Ivy"},
            new(){Seller = "Jack"},
            new(){Seller = "Katie"},
            new(){Seller = "Leo"},
            new(){Seller = "Mia"},
        };

    public static IEnumerable<Product> GetDefaultsProducts() =>
        new Product[]
        {

              new()
            {
                Name = "iPhone X",
                Description = "Description for iPhone X",
                ImageUrl = "https://example.com/images/iphone_x.png",
                Price = rand.NextDouble() % 1000,
                ProductTypeId = 2,
                ProductBrandId = 2,
                ProductSellerId = rand.Next(1,10),
            },
            new Product
            {
                Name = "BMW X10",
                Description = "Description for BMW X10",
                ImageUrl = "https://example.com/images/bmw_x10.png",
                Price = rand.NextDouble() % 1000,
                ProductTypeId = 7,
                ProductBrandId = 8,
                ProductSellerId = rand.Next(1,10),
            },
            new Product
            {
                Name = "Samsung Galaxy S21",
                Description = "Description for Samsung Galaxy S21",
                ImageUrl = "https://example.com/images/samsung_s21.png",
                Price = rand.NextDouble() % 1000,
                ProductTypeId = 2,
                ProductBrandId = 7,
                ProductSellerId = rand.Next(1,10),
            },
            new Product
            {
                Name = "Rolex Submariner",
                Description = "Description for Rolex Submariner",
                ImageUrl = "https://example.com/images/rolex_submariner.png",
                Price = rand.NextDouble() % 1000,
                ProductTypeId = 1,
                ProductBrandId = 1,
                ProductSellerId = rand.Next(1,10),
            },
            new Product
            {
                Name = "Coca Cola Classic",
                Description = "Description for Coca Cola Classic",
                ImageUrl = "https://example.com/images/coca_cola_classic.png",
                Price = rand.NextDouble() % 1000,
                ProductTypeId = 3,
                ProductBrandId = 3,
                ProductSellerId = rand.Next(1,10),
            },
            new Product
            {
                Name = "Audi A4",
                Description = "Description for Audi A4",
                ImageUrl = "https://example.com/images/audi_a4.png",
                Price = rand.NextDouble() % 1000,
                ProductTypeId = 7,
                ProductBrandId = 9,
                ProductSellerId = rand.Next(1,10),
            },
            new Product
            {
                Name = "Loreal Paris Shampoo",
                Description = "Description for Loreal Paris Shampoo",
                ImageUrl = "https://example.com/images/loreal_shampoo.png",
                Price = rand.NextDouble() % 1000,
                ProductTypeId = 5,
                ProductBrandId = 6,
                ProductSellerId = rand.Next(1,10),
            },
            new Product
            {
                Name = "Dell XPS 15",
                Description = "Description for Dell XPS 15",
                ImageUrl = "https://example.com/images/dell_xps_15.png",
                Price = rand.NextDouble() % 1000,
                ProductTypeId = 8,
                ProductBrandId = 10,
                ProductSellerId = rand.Next(1,10),
            },
            new Product
            {
                Name = "Pepsi Max",
                Description = "Description for Pepsi Max",
                ImageUrl = "https://example.com/images/pepsi_max.png",
                Price = rand.NextDouble() % 1000,
                ProductTypeId = 2,
                ProductBrandId = 4,
                ProductSellerId = 4,
            },
            new Product
            {
                Name = "Sony PlayStation 5",
                Description = "Description for Sony PlayStation 5",
                ImageUrl = "https://example.com/images/sony_ps5.png",
                Price = rand.NextDouble() % 1000,
                ProductTypeId = 9,
                ProductBrandId = 12,
                ProductSellerId = rand.Next(1,10),
            },
        };
}