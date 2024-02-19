using System.Security.Claims;

namespace eShop.Services.Identity.API.Extensions;

public static class ClaimsPrincipalExtension
{

    public static string FindFirstIdentityValue(this ClaimsPrincipal claimsPrincipal) =>
        claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier) ??
            claimsPrincipal.FindFirstValue(ClaimTypes.Name) ??
                throw new Exception("user credential not found");

    public static string FindFirstMailValue(this ClaimsPrincipal claimsPrincipal) =>
        claimsPrincipal.FindFirstValue(ClaimTypes.Email) ??
                throw new Exception("email not found");
}