using AutoMapper;
using eShop.MVC.Models.Classes;
using eShop.MVC.Protos.Cart;

namespace eShop.MVC.AutoMapperProfile;

public class CartAutoMapperProfile: Profile
{
    public CartAutoMapperProfile()
    {
        CreateMap<CartProductDto, CartProduct>()
            .ForMember(cP => cP.ObjectId, opt => opt.MapFrom(cPD => cPD.ObjectId))
            .ForMember(cP => cP.Quantity, opt => opt.MapFrom(cPD => cPD.Quantity))
            .ForMember(cP => cP.ProductId, opt => opt.MapFrom(cPD => cPD.ProductId))
            .ForMember(cP => cP.ProductName, opt => opt.MapFrom(cPD => cPD.ProductName))
            .ForMember(cP => cP.ProductSeller, opt => opt.MapFrom(cPD => cPD.ProductSeller))
            .ForMember(cP => cP.ProductBrand, opt => opt.MapFrom(cPD => cPD.ProductBrand))
            .ForMember(cP => cP.ImageUrl, opt => opt.MapFrom(cPD => cPD.ImageUrl))
            .ForMember(cP => cP.Price, opt => opt.MapFrom(cPD => cPD.Price))
            .ReverseMap();

        CreateMap<CartDto, Cart>()
            .ForMember(cD => cD.CartProducts, opt => opt.MapFrom(cPD => cPD.CartProducts))
            .ForMember(cD => cD.Coupon, opt => opt.MapFrom(cDD => cDD.Coupon))
            .ReverseMap();
    }
}
