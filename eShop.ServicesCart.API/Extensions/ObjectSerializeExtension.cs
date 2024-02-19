using System.Text.Json;

namespace eShop.ServicesCart.API.Extensions;

public static class ObjectSerializeExtension
{
    public static string ToJsonSerailize<T>(this T obj) =>
        JsonSerializer.Serialize(obj);

    public static T? ToJsonDeserialize<T>(this string json) =>
        JsonSerializer.Deserialize<T>(json);
}