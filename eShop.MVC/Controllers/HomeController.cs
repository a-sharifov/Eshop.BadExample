using eShop.MVC.Builders;
using eShop.MVC.Constants;
using eShop.MVC.Models.Classes;
using eShop.MVC.Protos.Product;
using eShop.MVC.Services.Intefaces;
using eShop.MVC.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace eShop.MVC.Controllers;

public class HomeController : Controller
{
    private readonly IProductService _productService;
    private readonly IProductPaginationService _productPaginationService;
    private HomeIndexViewModel _viewModel;

    public HomeController(IProductService productService, IProductPaginationService paginationService) =>
        (_productService, _productPaginationService, _viewModel) = (productService, paginationService, new());
    
    public async Task<IActionResult> Index(FilterRequestFind filterRequestFind)
    {
        return await GetProductsViewModelAsync(filterRequestFind);
    }

    public async Task<IActionResult> SelectedPage(FilterRequestFind filterRequestFind)
    {
        var filter = new FilterRequest()
        {
            ProductName = filterRequestFind.ProductName ,
            IncludeTables = "ProductBrand,ProductType",
            IncludeFields = "Id, Name, Price, Count, ImageUrl",
            Skip = (filterRequestFind.Skip - 1) * SD.TakeDefault,
            Take = SD.TakeDefault,
        };

        return await GetProductsViewModelAsync(filter);
    }

    private async Task<IActionResult> GetProductsViewModelAsync(FilterRequestFind filterRequestFind)
    {
        var paginationResult = await _productPaginationService.GetPaginationAsync(
            FilterRequestBuilder.Build(filterRequestFind,
            IncludeTables: "ProductBrand,ProductType",
            IncludeFields: "Id, Name, Price, Count, ImageUrl"));


        SaveFilterRequestFind(filterRequestFind);
        return await GetProductsViewModelAsync(paginationResult);
    }

    private async Task<IActionResult> GetProductsViewModelAsync(FilterRequest request) =>
        await GetProductsViewModelAsync(
            await _productPaginationService.GetPaginationAsync(request));

    private async Task<IActionResult> GetProductsViewModelAsync(PaginationResult<Product> paginationResult)
    {
        _viewModel = new(
            await _productService.GetAllBrandAsync(),
            await _productService.GetAllTypeAsync(),
            paginationResult);

        return View("Index", _viewModel);
    }

    public IActionResult Privacy() => View();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() =>
        View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

    [Authorize]
    public IActionResult Login() => 
        RedirectToAction("Index");

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> ProductDetails(int productId) =>
        View(await _productService.GetProductAsync(productId,
            includeTables: "ProductBrand,ProductType,ProductSeller"));

    public void SaveFilterRequestFind(FilterRequestFind filterRequestFind)
    {
        var filterRequestFindType = typeof(FilterRequestFind);
        var filterRequestFindConstantsType = typeof(FilterRequestFindConstants);

        foreach (var item in filterRequestFindConstantsType.GetFields())
        {
            ViewData[item.Name] = filterRequestFindType.GetProperty(item.Name)
                                                       .GetValue(filterRequestFind);
        }
    }

}