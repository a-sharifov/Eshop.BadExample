using eShop.ServicesCart.API.Constants;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace eShop.ServicesCart.API.Models;

[BsonIgnoreExtraElements]
public class CartProductIndex
{
    [BsonElement(CartProductConstants.ObjectId), BsonId]
    public ObjectId ObjectId { get; set; }

    [BsonElement(CartProductConstants.ProductId)]
    public int ProductId { get; set; }

    [BsonElement(CartProductConstants.Quantity)]
    public int Quantity { get; set; }
}