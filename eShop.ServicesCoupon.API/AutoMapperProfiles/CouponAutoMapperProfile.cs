using AutoMapper;
using eShop.ServicesCoupon.API.Models;
using eShop.ServicesCoupon.API.Protos.Coupon;

namespace eShop.ServicesCoupon.API.AutoMapperProfiles;

public class CouponAutoMapperProfile : Profile
{
    public CouponAutoMapperProfile()
    {
        CreateMap<CouponDto, Coupon>()
            .ReverseMap();
    }
}
