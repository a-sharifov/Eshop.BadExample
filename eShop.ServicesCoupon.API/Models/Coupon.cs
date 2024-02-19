
namespace eShop.ServicesCoupon.API.Models;

public class Coupon
{
    public int Id { get; set; }
    public string CouponCode { get; set; } = null!;
    public double Discount { get; set; }
}
