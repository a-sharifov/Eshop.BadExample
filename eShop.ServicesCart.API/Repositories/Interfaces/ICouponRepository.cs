
namespace eShop.ServicesCart.API.Repositories.Interfaces;

public interface ICouponRepository
{
    public Task<bool> CheckCouponCodeAsync(string couponCode);
    public Task<double?> GetDiscountAsync(string couponCode);
}