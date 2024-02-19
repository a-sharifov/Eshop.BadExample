using eShop.ServicesCart.API.Databases.Configurations;
using MongoDB.Driver;

namespace eShop.ServicesCart.API.Extensions;

public static class MongoClientExtension
{
    public static IMongoCollection<T> GetCollection<T>(this IMongoClient mongoClient, DatabaseSettings settings) =>
            mongoClient.GetDatabase(settings.DatabaseName).GetCollection<T>(settings.CollectionName);
}
