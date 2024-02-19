using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using IdentityModel;

namespace eShop.Services.Identity.API;

public static class SD
{
    public const string DefaultScheme = "Cookie";
    public enum Role : byte
    {
        Admin,
        User,
    }

    public static IEnumerable<IdentityResource> IdentityResources =>
        new List<IdentityResource>
        {
            new IdentityResources.OpenId(),

            new IdentityResources.Email(),
            new IdentityResources.Profile(),
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new List<ApiScope>
        {
            new ApiScope("eShop.MVC", "eShop.MVC server"),
            new ApiScope("read", "Read server"),
            new ApiScope("write", "Write server"),
            new ApiScope("delete", "Delete server"),
        };

    public static IEnumerable<Client> Clients =>
        new List<Client>
        {
            new Client
            {
                ClientId = "eShop.MVC",
                ClientSecrets = {new Secret("secret".Sha256())},
                AllowedGrantTypes = GrantTypes.Code,
                RedirectUris = {@"https://localhost:7195/signin-oidc" },
                PostLogoutRedirectUris = { @"https://localhost:7195/signout-callback-oidc" },
                AllowedScopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.Email,
                    JwtClaimTypes.Role,
                },
            },
        };
}
