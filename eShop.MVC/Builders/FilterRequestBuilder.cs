using eShop.MVC.Models.Classes;
using eShop.MVC.Protos.Product;

namespace eShop.MVC.Builders;

public class FilterRequestBuilder
{
    public static FilterRequest Build(FilterRequestFind requestFind, string IncludeFields, string? IncludeTables = null)
    {
        return new FilterRequest
        {
            ProductBrand = requestFind.ProductBrand,
            ProductName = requestFind.ProductName,
            ProductSeller = requestFind.ProductSeller,
            ProductType = requestFind.ProductType,
            MaxPrice = requestFind.MaxPrice,
            MinPrice = requestFind.MinPrice,
            IncludeFields = IncludeFields,
            IncludeTables = IncludeTables,
            Skip = requestFind.Skip,
            Take = requestFind.Take,
        };
    }
}
