using eShop.ServicesCart.API.Constants;
using eShop.ServicesCart.API.Databases.Configurations;
using eShop.ServicesCart.API.Models;
using eShop.ServicesCart.API.Models.Messages;
using eShop.ServicesCart.API.Repositories.Interfaces;
using eShop.ServicesCart.API.Validations;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace eShop.ServicesCart.API.Repositories.Classes;

public class CartRepository : ICartRepository
{
    private readonly ICartRedisRepository _redisRepository;
    private readonly IProductRepository _productRepository;
    private readonly ICouponRepository _couponRepository;
    private readonly IMongoCollection<BsonDocument> _cartCollection;
    private readonly CheckoutHeaderValidator _checkoutHeaderValidator;
    private const int expireMinutes = 15;

    public CartRepository(IOptions<DatabaseSettings> options,
                          IProductRepository productRepository,
                          ICouponRepository couponRepository,
                          IMongoClient mongoClient,
                          ICartRedisRepository redisRepository,
                          CheckoutHeaderValidator checkoutHeaderValidator)
    {
        _productRepository = productRepository;
        _redisRepository = redisRepository;
        _couponRepository = couponRepository;
        _checkoutHeaderValidator = checkoutHeaderValidator;
        var optionsValue = options.Value;

        _cartCollection = mongoClient.GetDatabase(optionsValue.DatabaseName)
                                      .GetCollection<BsonDocument>(optionsValue.CollectionName);
    }

    public async Task<Cart> GetCartAsync(string userName)
    {
        var cart = await GetCartIfExistsAsync(userName);

        if (cart != null)
        {
            await _redisRepository.SetCartAsync(cart, expireMinutes);

            return cart;
        }

        await CreateCartAsync(userName);

        return await GetCartAsync(userName);
    }

    public async Task<bool> AddCouponInCartAsync(string userName, string couponCode)
    {
        var isExists = await _couponRepository.CheckCouponCodeAsync(couponCode);

        if (!isExists)
        {
            return false;
        }

        var cart = await GetCartAsync(userName);
        var filter = FilterDefinitionUserCart<BsonDocument>(cart.ObjectId);
        var update = Builders<BsonDocument>.Update.Set(CartConstants.CouponCode, couponCode);
        await _cartCollection.UpdateOneAsync(filter, update);
        return true;
    }

    public async Task<bool> DeleteCouponInCartAsync(string userName)
    {
        var cart = await GetCartAsync(userName);
        var filter = FilterDefinitionUserCart<BsonDocument>(cart.ObjectId);
        var update = Builders<BsonDocument>.Update.Unset(CartConstants.CouponCode);
        var updateResponse = await _cartCollection.UpdateOneAsync(filter, update);
        return updateResponse.IsModifiedCountAvailable;
    }

    private async Task CreateCartAsync(string userName)
    {
        var newCartDocument = new BsonDocument()
        {
            { CartConstants.UserName, userName },
            { CartConstants.CartProductIndexes, new BsonArray() }
        };

        await _cartCollection.InsertOneAsync(newCartDocument);
    }

    private async Task<Cart?> GetCartIfExistsAsync(string userName)
    {
        var cart = await GetCartIfExistsRedisAsync(userName);

        if (cart != null)
        {
            return cart;
        }

        return await GetCartIfExistsMongoAsync(userName);
    }

    private async Task<Cart?> GetCartIfExistsRedisAsync(string userName) =>
        await _redisRepository.GetCartAsync(userName, expireMinutes);

    private async Task<Cart?> GetCartIfExistsMongoAsync(string userName)
    {
        var cartDocument = await FirstOrDefaultCartAsync(
            FilterDefinitionUserCart<BsonDocument>(userName));

        return cartDocument == null ? null
            : BsonSerializer.Deserialize<Cart>(cartDocument);
    }


    public async Task<int> DeleteProductInCartByIndexAsync(string userName, string objectId)
    {
        var cart = await GetCartAsync(userName);
        var filter = FilterDefinitionUserCart<BsonDocument>(objectId);
        var update = Builders<BsonDocument>.Update
            .PullFilter(CartConstants.CartProductIndexes, Builders<BsonDocument>.Filter.Eq(CartProductConstants.ObjectId, objectId));

        var updateResult = await _cartCollection.UpdateOneAsync(filter, update);

        return (int)updateResult.ModifiedCount;
    }

    public async Task<int> DeleteProductsInCartAsync(string userName)
    {
        var cart = await GetCartAsync(userName);
        var update = Builders<BsonDocument>.Update.Set(CartConstants.CartProductIndexes, new List<CartProductIndex>());
        var updateResult = await _cartCollection.UpdateOneAsync(
            FilterDefinitionUserCart<BsonDocument>(cart.ObjectId), update);

        await _redisRepository.SetCartAsync(cart, expireMinutes);

        return (int)updateResult.ModifiedCount;
    }

    public async Task<int> AddProductInCartAsync(string userName, CartProductIndex product)
    {
        var cart = await GetCartAsync(userName);
        var update = Builders<BsonDocument>.Update.Push(CartConstants.CartProductIndexes, product);
        var updateResult = await _cartCollection.UpdateOneAsync(
                FilterDefinitionUserCart<BsonDocument>(cart.ObjectId), update);

        return (int)updateResult.ModifiedCount;
    }

    public async Task<int> AddCountInCartAsync(string userName, string objectId, int count)
    {
        var cart = await GetCartAsync(userName);
        var filter = Builders<BsonDocument>.Filter.And(
             FilterDefinitionUserCart<BsonDocument>(objectId),
             Builders<BsonDocument>.Filter.Eq($"{CartConstants.CartProductIndexes}.{CartProductConstants.ObjectId}", objectId)
            );

        var update = Builders<BsonDocument>.Update.Inc($"{CartConstants.ObjectId}.$.{CartProductConstants.Quantity}", count);

        var updateResult = await _cartCollection.UpdateOneAsync(filter, update);

        return (int)updateResult.ModifiedCount;
    }


    [Obsolete("доделай срочно")]
    public async Task<bool> CheckoutAsync(CheckoutHeader checkoutHeader)
    {
        var userName = checkoutHeader.UserName;
        var cart = await GetCartAsync(userName);
        var cartIsNullOrEmpty = cart.CartProductIndexes.IsNullOrEmpty();

        if (cartIsNullOrEmpty)
        {
            return false;
        }

        var couponCode = checkoutHeader.CouponCode;
        var discount = await _couponRepository.GetDiscountAsync(couponCode);

        if (discount != null && discount != checkoutHeader.Discount)
        {
            return false;
        }

        checkoutHeader.CartProducts = await
            _productRepository.GetProductsAsync(cart.CartProductIndexes);

        var validationResult = await _checkoutHeaderValidator.ValidateAsync(checkoutHeader);

        if (!validationResult.IsValid)
        {
            return false;
        }
        

        await DeleteProductsInCartAsync(userName);
        return true;
    }


    private static FilterDefinition<T> FilterDefinitionUserCart<T>(string userName) =>
          Builders<T>.Filter.Eq(CartConstants.UserName, userName);

    private static FilterDefinition<T> FilterDefinitionUserCart<T>(ObjectId objectId) =>
         Builders<T>.Filter.Eq(CartConstants.ObjectId, objectId);

    private async Task<BsonDocument?> FirstOrDefaultCartAsync(FilterDefinition<BsonDocument> filterDefinition)
    {
        var cartCursor = await _cartCollection.FindAsync(filterDefinition);
        return await cartCursor.FirstOrDefaultAsync();
    }

}