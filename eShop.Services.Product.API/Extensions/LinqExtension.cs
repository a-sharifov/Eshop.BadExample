using Microsoft.EntityFrameworkCore;

namespace eShop.ServicesProduct.API.Extensions;

public static class LinqExtension
{
    public static IQueryable<T> DynamicInclude<T>(this IQueryable<T> queryable, string? includeString) 
        where T : class
    {
        var includes = includeString!.CommaSplit();
        if (includes == null)
        {
            return queryable;
        }

        foreach (var item in includes)
        {
           queryable = queryable.Include(item);
        }

        return queryable;
    }
}
