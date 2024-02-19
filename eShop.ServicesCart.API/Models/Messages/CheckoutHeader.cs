namespace eShop.ServicesCart.API.Models.Messages;

public class CheckoutHeader
{
    public int Id { get; set; }
    public string UserName { get; set; } = null!;
    public string City { get; set; } = null!;
    public string Street { get; set; } = null!;
    public string State { get; set; } = null!;
    public string Country { get; set; } = null!;
    public string ZipCode { get; set; } = null!;
    public string CouponCode { get; set; } = null!;
    public double Discount { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public DateTime CardExpiration { get; set; }
    public string Phone { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string CardNumber { get; set; } = null!;
    public string CVV { get; set; } = null!;
    public double OrderTotal { get; set; }
    public IEnumerable<CartProduct> CartProducts { get; set; } = null!;
}