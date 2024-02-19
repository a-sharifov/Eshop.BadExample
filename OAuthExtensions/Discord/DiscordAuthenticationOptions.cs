using eShop.Services.Identity.OAuthExtensions.Discord;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using System.Security.Claims;

public class DiscordAuthenticationOptions : OAuthOptions
{
    public string? Prompt { get; set; }

    public DiscordAuthenticationOptions()
    {
        ClaimsIssuer = DiscordAuthenticationDefaults.Issuer;
        CallbackPath = DiscordAuthenticationDefaults.CallbackPath;
        AuthorizationEndpoint = DiscordAuthenticationDefaults.AuthorizationEndpoint;
        TokenEndpoint = DiscordAuthenticationDefaults.TokenEndpoint;
        UserInformationEndpoint = DiscordAuthenticationDefaults.UserInformationEndpoint;

        ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
        ClaimActions.MapJsonKey(ClaimTypes.Name, "username");
        ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
        ClaimActions.MapJsonKey(ClaimTypes.GivenName, "username");
        ClaimActions.MapJsonKey(ClaimTypes.Surname, "discriminator");
        ClaimActions.MapJsonKey(ClaimTypes.Uri, "avatar");
        ClaimActions.MapJsonKey(ClaimTypes.Country, "locale");

        Scope.Add("identify");
        Scope.Add("email");
    }
}
