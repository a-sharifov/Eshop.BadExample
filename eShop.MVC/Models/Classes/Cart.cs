namespace eShop.MVC.Models.Classes;

public class Cart
{
    public List<CartProduct> CartProducts { get; set; } = null!;
    public string? Coupon { get; set; }
}