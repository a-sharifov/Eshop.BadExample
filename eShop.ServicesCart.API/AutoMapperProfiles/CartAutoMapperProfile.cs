using AutoMapper;
using eShop.ServicesCart.API.Models;
using eShop.ServicesCart.API.Protos.Cart;

namespace eShop.ServicesCart.API.AutoMapperProfiles;

public class CartAutoMapperProfile : Profile
{
    public CartAutoMapperProfile()
    {
        CreateMap<CartProductDto, CartProduct>()
            .ReverseMap();

        CreateMap<CartProductIndex, CartProductDto>()
            .ForMember(c => c.ObjectId, opt => opt.MapFrom(cD => cD.ObjectId.ToString()))
            .ReverseMap();

        CreateMap<CartDto, Cart>()
            .ReverseMap();

        CreateMap<AddProductInCartRequest, CartProductIndex>()
            .ReverseMap();
    }
}
