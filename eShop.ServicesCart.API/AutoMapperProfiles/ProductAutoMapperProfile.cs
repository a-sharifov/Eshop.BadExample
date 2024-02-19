using AutoMapper;
using eShop.ServicesCart.API.Models;
using eShop.ServicesCart.API.Protos.Product;

namespace eShop.ServicesCart.API.AutoMapperProfiles;

public class ProductAutoMapperProfile : Profile
{
    public ProductAutoMapperProfile()
    {
        CreateMap<ProductDto, CartProductIndex>()
            .ForMember(cP => cP.ProductId, opt => opt.MapFrom(pD => pD.Id))
            .ReverseMap();

        CreateMap<ProductDto, CartProduct>()
           .ForMember(p => p.ProductId, opt => opt.MapFrom(pD => pD.Id))
           .ForMember(p => p.ProductName, opt => opt.MapFrom(pD => pD.Name))
           .ForMember(p => p.ProductBrand, opt => opt.MapFrom(pD => pD.ProductBrandDto.Brand))
           .ForMember(p => p.ProductSeller, opt => opt.MapFrom(pD => pD.ProductSellerDto.Seller))
           .ReverseMap();
    }

}