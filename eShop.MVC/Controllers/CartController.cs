using eShop.MVC.Models.Classes;
using eShop.MVC.Services.Intefaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eShop.MVC.Controllers;

public class CartController : Controller
{
    private readonly ICartService _cartService;
    private readonly IProductService _productService;

    public CartController(ICartService cartService, IProductService productService) =>
        (_cartService, _productService) = (cartService, productService);

    [Authorize]
    public async Task<IActionResult> CartIndex()
    {

        var cart = await _cartService.GetCartAsync(
            User.Identity.Name, await HttpContext.GetTokenAsync("access_token"));

        return View(cart);
    }

    [HttpPost, Authorize]
    public async Task<IActionResult> AddProduct(int productId, int quantity)
    {
        await _cartService.AddProductInCartAsync(
            User.Identity.Name,
            productId,
            quantity,
            await GetTokenAsync()
            );

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public async Task<IActionResult> DeleteProducts()
    {
        await _cartService.DeleteProductsAsync(
            User.Identity.Name,
            await GetTokenAsync()
            );

        return RedirectToAction("Index", "Home");
    }

  //  [HttpPost]
    public async Task<IActionResult> DeleteProductByIndex(string objectId)
    {
        await _cartService.DeleteProductInCartByIndexAsync(
            User.Identity.Name,
            objectId,
            await GetTokenAsync()
            );

        return RedirectToAction("Index", "Home");
    }

    private async Task<string?> GetTokenAsync(string tokenName = "access_token") =>
        await HttpContext.GetTokenAsync(tokenName);
}