using eShop.MVC.Models.Classes;
using eShop.MVC.Protos.Product;

namespace eShop.MVC.ViewModels;

public class HomeIndexViewModel
{
    public FilterRequest Filter { get; set; } = new();
    public PaginationResult<Product>? Pagination { get; set; } 
    public IEnumerable<Product> Products { get; set; } = null!;
    public IEnumerable<ProductBrand> Brands { get; set; } = null!;
    public IEnumerable<ProductType> Types { get; set; } = null!;

    public HomeIndexViewModel()
    {
        
    }

    public HomeIndexViewModel(IEnumerable<ProductBrand> brands,
        IEnumerable<ProductType> types, PaginationResult<Product> paginationResult)
    {
        Brands = brands;
        Types = types;
        Products = paginationResult.Items;
        Pagination = paginationResult;
    }
}