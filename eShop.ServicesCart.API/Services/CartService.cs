using AutoMapper;
using eShop.ServicesCart.API.Models;
using eShop.ServicesCart.API.Protos.Cart;
using eShop.ServicesCart.API.Repositories.Interfaces;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using static eShop.ServicesCart.API.Protos.Cart.DefaultCartService;

namespace eShop.ServicesCart.API.Services;

[Authorize]
public class CartService: DefaultCartServiceBase
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public CartService(ICartRepository cartRepository, IProductRepository productRepository, IMapper mapper) =>
        (_cartRepository, _productRepository, _mapper) = (cartRepository, productRepository, mapper);

    public override async Task<CountResponse> AddCountInCart(CountInCartRequest request, ServerCallContext context)
    {
        var userName = request.UserName;
        var objectId = request.ObjectId;
        var count = request.Count;

        var result = await _cartRepository.AddCountInCartAsync(userName, objectId, count);

        return new CountResponse { Count = result };
    }

    public override async Task<CountResponse> AddProductInCart(AddProductInCartRequest request, ServerCallContext context)
    {
        var userName = request.UserName;
        var cartProduct = _mapper.Map<CartProductIndex>(request);

        var result = await _cartRepository.AddProductInCartAsync(userName, cartProduct);

        return new CountResponse { Count = result };
    }

    public override async Task<CountResponse> DeleteProductsInCart(UserNameRequest request, ServerCallContext context)
    {
        var userName = request.UserName;

        var deleteCount = await _cartRepository.DeleteProductsInCartAsync(userName);

        return new CountResponse { Count = deleteCount };
    }

    public override async Task<CountResponse> DeleteProductInCartByIndex(UserNameIndexRequest request, ServerCallContext context)
    {
        var userName = request.UserName;
        var objectId = request.ObjectId;

        var deleteCount = await _cartRepository.DeleteProductInCartByIndexAsync(userName, objectId);

        return new CountResponse { Count = deleteCount };
    }

    public override async Task<CartDto> GetCart(UserNameRequest request, ServerCallContext context)
    {
        var cart = await _cartRepository.GetCartAsync(request.UserName);

        var cartDto = _mapper.Map<CartDto>(cart);
        cartDto.CartProducts.Clear();

        cartDto.CartProducts.AddRange(_mapper.Map<IEnumerable<CartProductDto>>(
            await _productRepository.GetProductsAsync(cart.CartProductIndexes)));

        return cartDto;
    }

    public override async Task<AddCouponInCartResponse> AddCouponInCart(AddCouponInCartRequest request, ServerCallContext context)
    {
        var couponCode = request.CouponCode;
        var userName = request.UserName;

        var isAdded = await _cartRepository.AddCouponInCartAsync(userName, couponCode);

        return new AddCouponInCartResponse { IsAdded = isAdded };
    }

    public override async Task<DeleteCouponInCartResponse> DeleteCouponInCart(UserNameRequest request, ServerCallContext context)
    {
        var userName = request.UserName;
        var isDeleted = await _cartRepository.DeleteCouponInCartAsync(userName);

        return new DeleteCouponInCartResponse { IsDeleted = isDeleted };
    }
    
}