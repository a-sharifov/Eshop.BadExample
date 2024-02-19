using AutoMapper;
using eShop.MVC.Models.Classes;
using eShop.MVC.Protos.Product;
using eShop.MVC.ViewModels;

namespace eShop.MVC.AutoMapperProfile;

public class ProductAutoMapperProfile : Profile
{
    public ProductAutoMapperProfile()
    {
        CreateMap<ProductBrandDto, ProductBrand>()
            .ReverseMap();

        CreateMap<ProductTypeDto, ProductType>()
            .ReverseMap();

        CreateMap<ProductSellerDto, ProductSeller>()
           .ReverseMap();

        CreateMap<ProductDto, Product>()
           .ForMember(p => p.ProductTypeId, opt => opt.MapFrom(pD => pD.ProductTypeDto.Id))
           .ForMember(p => p.ProductBrandId, opt => opt.MapFrom(pD => pD.ProductBrandDto.Id))
           .ForMember(p => p.ProductSellerId, opt => opt.MapFrom(pD => pD.ProductSellerDto.Id))
           .ForMember(p => p.ProductType, opt => opt.MapFrom(pD => pD.ProductTypeDto))
           .ForMember(p => p.ProductBrand, opt => opt.MapFrom(pD => pD.ProductBrandDto))
           .ForMember(p => p.ProductSeller, opt => opt.MapFrom(pD => pD.ProductSellerDto))
           .ReverseMap();

        CreateMap<ProductViewModel, Product>()
            .ForPath(p => p.ProductType.Type, opt => opt.MapFrom(pVM => pVM.ProductType))
            .ForPath(p => p.ProductBrand.Brand, opt => opt.MapFrom(pVM => pVM.ProductBrand))
            .ForPath(p => p.ProductSeller.Seller, opt => opt.MapFrom(pVM => pVM.ProductSeller))
            .ReverseMap();
    }
}