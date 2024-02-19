using eShop.ServicesCart.API.Constants;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace eShop.ServicesCart.API.Models;

[BsonIgnoreExtraElements]
public class Cart
{
    [BsonElement(CartConstants.ObjectId), BsonId]
    public ObjectId ObjectId { get; set; }

    [BsonElement(CartConstants.UserName), BsonRequired]
    public string UserName { get; set; } = null!;

    [BsonElement(CartConstants.CartProductIndexes), BsonRequired]
    public IList<CartProductIndex> CartProductIndexes { get; set; } = null!;

    [BsonElement(CartConstants.CouponCode), BsonIgnoreIfNull]
    public string? CouponCode { get; set; }

}