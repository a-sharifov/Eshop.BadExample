namespace eShop.ServicesProduct.API.Extensions;

public static class ListExtension
{
    public static void AddIfNotNull<T>(this IList<T> list, T? value)
    {
        if(value != null)
        {
            list.Add(value);
        }
    }

}
