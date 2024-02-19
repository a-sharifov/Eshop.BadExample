namespace eShop.Services.Identity.OAuthExtensions.GitLab;

public class GitLabAuthenticationDefaults
{
    public const string AuthenticationScheme = "GitLab";
    public static readonly string DisplayName = "GitLab";
    public static readonly string Issuer = "GitLab";
    public static readonly string CallbackPath = "/signin-gitlab";
    public static readonly string AuthorizationEndpoint = "https://gitlab.com/oauth/authorize";
    public static readonly string TokenEndpoint = "https://gitlab.com/oauth/token";
    public static readonly string UserInformationEndpoint = "https://gitlab.com/api/v4/user";
}
