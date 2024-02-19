namespace eShop.ServicesProduct.API.Models;

public class Product
{
    public int Id { get; set; }
    public double Price { get; set; }
    public string Name { get; set; } = null!;
    public string? Composition { get; set; }
    public string? Description { get; set; }
    public string ImageUrl { get; set; } = null!;
    public int ProductSellerId { get; set; }
    public ProductSeller ProductSeller { get; set; } = null!;
    public int ProductTypeId { get; set; }
    public ProductType ProductType { get; set; } = null!;
    public int ProductBrandId { get; set; }
    public ProductBrand ProductBrand { get; set; } = null!;
}