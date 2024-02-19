using AutoMapper;
using eShop.ServicesCart.API.Models;
using eShop.ServicesCart.API.Protos.Coupon;
using eShop.ServicesCart.API.Repositories.Interfaces;
using static eShop.ServicesCart.API.Protos.Coupon.DefaultCouponService;

namespace eShop.ServicesCart.API.Repositories.Classes;

public class CouponRepository : ICouponRepository
{
    private readonly DefaultCouponServiceClient _couponService;
    private readonly IMapper _mapper;

    public CouponRepository(DefaultCouponServiceClient couponService, IMapper mapper) =>
        (_couponService, _mapper) = (couponService, mapper);


    public async Task<bool> CheckCouponCodeAsync(string couponCode)
    {
        var request = CreateCouponRequest(couponCode);
        var couponDto = await _couponService.GetCouponAsync(request);
        return couponDto != null;
    }

    public async Task<double?> GetDiscountAsync(string couponCode)
    {
        var request = CreateCouponRequest(couponCode);
        var couponDto = await _couponService.GetCouponAsync(request);
        return couponDto!.Discount;
    }

    private CouponCodeRequest CreateCouponRequest(string couponCode) =>
        new() { CouponCode = couponCode };

}
