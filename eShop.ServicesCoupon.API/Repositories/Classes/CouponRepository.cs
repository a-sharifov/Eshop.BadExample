using eShop.ServicesCoupon.API.DbContexts;
using eShop.ServicesCoupon.API.Models;
using eShop.ServicesCoupon.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace eShop.ServicesCoupon.API.Repositories.Classes;

public class CouponRepository : ICouponRepository
{
    private readonly CouponDbContext _dbContext;

    public CouponRepository(CouponDbContext dbContext) =>
        _dbContext = dbContext;


    public async Task<Coupon?> GetCouponAsync(string couponCode)
    {
        return await _dbContext.Coupons.FirstOrDefaultAsync(c => c.CouponCode == couponCode);
    }

    public async Task<bool> CreateCouponAsync(Coupon coupon)
    {
        await _dbContext.AddAsync(coupon);
        var response = await _dbContext.SaveChangesAsync();
        return response != 0;
    }

    public async Task<bool> DeleteCouponAsync(string couponCode)
    {
        var coupon = await _dbContext.Coupons
            .Where(c => c.CouponCode == couponCode)
            .FirstAsync();

        _dbContext.Remove(coupon);
        var response = await _dbContext.SaveChangesAsync();
        return response != 0;
    }

    public async Task<IEnumerable<Coupon>> GetCouponsAsync()
    {
        return await _dbContext.Coupons
                          .AsNoTracking()
                          .ToListAsync();
    }
}
