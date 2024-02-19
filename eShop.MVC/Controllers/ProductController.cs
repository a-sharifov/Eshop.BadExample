using AutoMapper;
using eShop.MVC.Models.Classes;
using eShop.MVC.Protos.Product;
using eShop.MVC.Services.Intefaces;
using eShop.MVC.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace eShop.MVC.Controllers;

public class ProductController : Controller
{
    private readonly IProductService _productService;
    private readonly IMapper _mapper;

    public ProductController(IProductService productService, IMapper mapper) =>
        (_productService, _mapper) = (productService, mapper);

    public async Task<IActionResult> ProductIndex()
    {
        var filter = new FilterRequest
        {
            ProductSeller = User.Identity?.Name,
            IncludeTables = "ProductBrand,ProductType",
            IncludeFields = "Id, Name, Price, Count, ProductBrand, ProductType",
        };

        List<Product> products = new(await _productService.GetProductsByFilterAsync(filter));
        return View(products);
    }

    public IActionResult ProductCreate() =>
        View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ProductCreate(ProductViewModel model)
    {
        if (ModelState.IsValid)
        {
            model.ProductSeller = User.Identity.Name;
            var token = await GetTokenAsync("access_token");
            await _productService.AddProductAsync(
                _mapper.Map<Product>(model), token);
            return Redirect(nameof(ProductIndex));
        }
        return View(model);
    }

    public async Task<IActionResult> ProductUpdate(int productId)
    {
        var product = await _productService.GetProductAsync(productId, includeTables: "ProductBrand,ProductType,ProductSeller");
        return View(_mapper.Map<ProductViewModel>(product));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ProductUpdate(ProductViewModel model)
    {
        if (ModelState.IsValid)
        {
            var token = await GetTokenAsync("access_token");
            var res = await _productService.UpdateProductAsync(
                _mapper.Map<Product>(model), token);
            if (res == 1)
            {
                return Redirect(nameof(ProductIndex));
            }
        }
        return View();
    }

    public async Task<IActionResult> ProductDelete(int productId)
    {
        var product = await _productService.GetProductAsync(productId, "ProductBrand,ProductType");
        return View(_mapper.Map<ProductViewModel>(product));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ProductDelete(ProductViewModel model)
    {
        var token = await GetTokenAsync("access_token");
        var res = await _productService.DeleteProductAsync(model.Id, token);
        if (res == 1)
        {
            return Redirect(nameof(ProductIndex));
        }
        return View();
    }

    private async Task<string?> GetTokenAsync(string tokenName = "access_token") =>
        await HttpContext.GetTokenAsync(tokenName);
}