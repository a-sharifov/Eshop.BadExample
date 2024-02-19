namespace eShop.ServicesProduct.API.Models;

public class ProductBrand
{
    public int Id { get; set; }
    public string Brand { get; set; } = null!;
    public string? Description { get; set; }
}