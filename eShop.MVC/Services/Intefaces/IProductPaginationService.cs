using eShop.MVC.Models.Classes;
using eShop.MVC.Protos.Product;

namespace eShop.MVC.Services.Intefaces;

public interface IProductPaginationService
{
    public Task<PaginationResult<Product>> GetPaginationAsync(FilterRequest filter);
}