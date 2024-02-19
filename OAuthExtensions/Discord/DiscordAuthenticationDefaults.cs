namespace eShop.Services.Identity.OAuthExtensions.Discord;

public static class DiscordAuthenticationDefaults
{
    public const string AuthenticationScheme = "Discord";
    public static readonly string DisplayName = "Discord";
    public static readonly string Issuer = "Discord";
    public static readonly string CallbackPath = "/signin-discord";
    public static readonly string AuthorizationEndpoint = "https://discord.com/api/oauth2/authorize";
    public static readonly string TokenEndpoint = "https://discord.com/api/oauth2/token";
    public static readonly string UserInformationEndpoint = "https://discord.com/api/users/@me";
}