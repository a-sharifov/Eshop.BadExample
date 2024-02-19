using Nest;

namespace eShop.ServicesProduct.API.Models;

public class ProductIndex
{
    [Keyword]
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public double Price { get; set; }
    public string ProductBrand { get; set; } = null!;
    public string ProductType { get; set; } = null!;
    public string ProductSeller { get; set; } = null!;
}