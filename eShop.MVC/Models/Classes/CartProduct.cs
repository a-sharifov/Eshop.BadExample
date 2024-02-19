namespace eShop.MVC.Models.Classes;

public class CartProduct
{
    public string ObjectId { get; set; } = null!;
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public string ProductName { get; set; } = null!;
    public string ProductSeller { get; set; } = null!;
    public string ProductBrand { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;
    public double Price { get; set; }
}