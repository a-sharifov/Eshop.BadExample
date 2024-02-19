
namespace eShop.ServicesCart.API.Models;

public class CartProduct
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public string ProductSeller { get; set; } = null!;

    public string ProductBrand { get; set; } = null!;

    public string ImageUrl { get; set; } = null!;

    public double Price { get; set; }

    public int Quantity { get; set; }
}