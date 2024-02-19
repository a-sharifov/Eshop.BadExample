using eShop.ServicesCoupon.API.Models;
using System.Collections.Generic;

namespace eShop.ServicesCoupon.API.Repositories.Interfaces;

public interface ICouponRepository
{
    public Task<bool> CreateCouponAsync(Coupon coupon);
    public Task<bool> DeleteCouponAsync(string couponCode);
    public Task<Coupon?> GetCouponAsync(string couponCode);
    public Task<IEnumerable<Coupon>> GetCouponsAsync();
}