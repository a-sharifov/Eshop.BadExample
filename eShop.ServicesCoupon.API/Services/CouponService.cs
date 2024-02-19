using AutoMapper;
using eShop.ServicesCoupon.API.Models;
using eShop.ServicesCoupon.API.Protos.Coupon;
using eShop.ServicesCoupon.API.Repositories.Interfaces;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using static eShop.ServicesCoupon.API.Protos.Coupon.DefaultCouponService;

namespace eShop.ServicesCoupon.API.Services;

[Authorize]
public class CouponService : DefaultCouponServiceBase
{
    public readonly IMapper _mapper;
    public readonly ICouponRepository _cartRepository;

    public CouponService(IMapper mapper, ICouponRepository cartRepository) =>
        (_mapper, _cartRepository) = (mapper, cartRepository);


    [Authorize("Admin")]
    public override async Task<CreateCouponResponse> CreateCoupon(CouponDto request, ServerCallContext context)
    {
        var coupon = _mapper.Map<Coupon>(request);

        var isCreated = await _cartRepository.CreateCouponAsync(coupon);

        return new CreateCouponResponse { IsCreated = isCreated };
    }

    public override async Task<CouponDto> GetCoupon(CouponCodeRequest request, ServerCallContext context)
    {
        var couponCode = request.CouponCode;

        var coupon = await _cartRepository.GetCouponAsync(couponCode);

        return _mapper.Map<CouponDto>(coupon);
    }

    [Authorize("Admin")]
    public override async Task<DeleteCouponResponse> DeleteCoupon(CouponCodeRequest request, ServerCallContext context)
    {
        var couponCode = request.CouponCode;

        var isDeleted = await _cartRepository.DeleteCouponAsync(couponCode);

        return new DeleteCouponResponse { IsDeleted = isDeleted };
    }

    public override async Task<CouponsDto> GetCoupons(Empty request, ServerCallContext context)
    {
        var coupons = await _cartRepository.GetCouponsAsync();
        var couponDtos = _mapper.Map<IEnumerable<CouponDto>>(coupons);
        var couponsDto = new CouponsDto();

        foreach (var couponDto in couponDtos)
        {
            couponsDto.Coupons.Add(couponDto);
        }

        return couponsDto;
    }
}