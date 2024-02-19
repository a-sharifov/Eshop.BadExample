using AutoMapper;
using eShop.MVC.Exstensions;
using eShop.MVC.Models.Classes;
using eShop.MVC.Protos.Cart;
using eShop.MVC.Services.Intefaces;
using Grpc.Core;
using static eShop.MVC.Protos.Cart.DefaultCartService;

namespace eShop.MVC.Services.Classes;

public class CartService : ICartService
{
    private readonly DefaultCartServiceClient _cartService;
    private readonly IMapper _mapper;
    private readonly Empty _empty;

    public CartService(DefaultCartServiceClient cartService, IMapper mapper) =>
        (_cartService, _mapper, _empty) = (cartService, mapper, new());

    public async Task<Cart> GetCartAsync(string userName, string token) =>
       _mapper.Map<Cart>(await _cartService.GetCartAsync(
           new UserNameRequest { UserName = userName },
           MetadataExtension.CreateAuthorizationBearerMetadata(token)));

    public async Task<int> AddProductInCartAsync(string userName, int productId, int quantity, string token)
    {
        var countResponse = await _cartService.AddProductInCartAsync(new AddProductInCartRequest
        {
            UserName = userName,
            ProductId = productId,
            Quantity = quantity,
        }, CreateAuthorizationBearerMetadata(token));

        return countResponse.Count;
    }

    public async Task<int> DeleteProductInCartByIndexAsync(string userName, string objectId, string token)
    {
        var countResponse = await _cartService.DeleteProductInCartByIndexAsync(new UserNameIndexRequest
        {
            UserName = userName,
            ObjectId = objectId,
        }, CreateAuthorizationBearerMetadata(token));

        return countResponse.Count;
    }

    public async Task<int> DeleteProductsAsync(string userName, string token)
    {
        var countResponse = await _cartService.DeleteProductsInCartAsync(new UserNameRequest
        {
            UserName = userName,
        }, CreateAuthorizationBearerMetadata(token));

        return countResponse.Count;
    }

    public async Task<int> AddCountInCartAsync(string userName, string objectId, int count, string token)
    {
        var countResponse = await _cartService.AddCountInCartAsync(new CountInCartRequest
        {
            UserName = userName,
            ObjectId = objectId,
            Count = count,
        }, CreateAuthorizationBearerMetadata(token));

        return countResponse.Count;
    }


    private Metadata CreateAuthorizationBearerMetadata(string token) =>
        MetadataExtension.CreateAuthorizationBearerMetadata(token);
}