using System.Security.Claims;

namespace eShop.Services.Identity.API.Extensions;

public static class IEnumerableClaimExtension
{
    public static string? FirstOrDefaultValue(this IEnumerable<Claim> claims, string claimType) =>
        claims.FirstOrDefault(x => x.Type == claimType)?.Value;
}