using AutoMapper;
using eShop.ServicesProduct.API.Models;
using eShop.ServicesProduct.API.Protos.Product;

namespace eShop.ServicesProduct.API.AutoMapperProfiles;

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

        CreateMap<ProductIndex, Product>()
            .ForPath(p => p.ProductBrand.Brand, opt => opt.MapFrom(pI => pI.ProductBrand))
            .ForPath(p => p.ProductType.Type, opt => opt.MapFrom(pI => pI.ProductType))
            .ForPath(p => p.ProductSeller.Seller, opt => opt.MapFrom(pI => pI.ProductSeller))
            .ReverseMap();

        CreateMap<ProductIndex, ProductDto>()
            .ForPath(p => p.ProductBrandDto, opt => opt.MapFrom(pI => pI.ProductBrand))
            .ForPath(p => p.ProductTypeDto.Type, opt => opt.MapFrom(pI => pI.ProductType))
            .ForPath(p => p.ProductSellerDto.Seller, opt => opt.MapFrom(pI => pI.ProductSeller))
            .ReverseMap();

        CreateMap<ProductDto, Product>()
            .ForMember(p => p.ProductTypeId, opt => opt.MapFrom(pD => pD.ProductTypeDto.Id))
            .ForMember(p => p.ProductBrandId, opt => opt.MapFrom(pD => pD.ProductBrandDto.Id))
            .ForMember(p => p.ProductSellerId, opt => opt.MapFrom(pD => pD.ProductSellerDto.Id))
            .ForMember(p => p.ProductType, opt => opt.MapFrom(pD => pD.ProductTypeDto))
            .ForMember(p => p.ProductBrand, opt => opt.MapFrom(pD => pD.ProductBrandDto))
            .ForMember(p => p.ProductSeller, opt => opt.MapFrom(pD => pD.ProductSellerDto))
            .ReverseMap();

        CreateMap<SearchFilter, FilterRequest>()
         .ReverseMap();

        CreateMap<IdProductRequest, IdProduct>()
            .ReverseMap();

        CreateMap<IdProductsRequest, IdProducts>()
           .ForMember(iP => iP.Ids, opt => opt.MapFrom(iPR => new List<int>(iPR.Ids)))
           .ReverseMap();
    }
}
