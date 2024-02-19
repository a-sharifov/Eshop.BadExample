namespace eShop.Services.Identity.OAuthExtensions.GitHub;

public static class GitHubAuthenticationDefaults
{
    public const string AuthenticationScheme = "GitHub";
    public const string DisplayName = "GitHub";
    public const string Issuer = "GitHub";
    public const string CallbackPath = "/signin-github";
    public const string AuthorizationEndpoint = "https://github.com/login/oauth/authorize";
    public const string TokenEndpoint = "https://github.com/login/oauth/access_token";
    public const string UserInformationEndpoint = "https://api.github.com/user";
    public const string UserEmailsEndpoint = "https://api.github.com/user/emails";
    public const string EnterpriseApiPath = "/api/v3";
    public const string AuthorizationEndpointPath = "/login/oauth/authorize";
    public const string TokenEndpointPath = "/login/oauth/access_token";
    public const string UserInformationEndpointPath = "/user";
    public const string UserEmailsEndpointPath = "/user/emails";
}