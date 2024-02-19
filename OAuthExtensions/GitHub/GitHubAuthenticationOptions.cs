using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using System.Security.Claims;

namespace eShop.Services.Identity.OAuthExtensions.GitHub;

public class GitHubAuthenticationOptions : OAuthOptions
{
    public GitHubAuthenticationOptions()
    {
        ClaimsIssuer = GitHubAuthenticationDefaults.Issuer;
        CallbackPath = GitHubAuthenticationDefaults.CallbackPath;

        AuthorizationEndpoint = GitHubAuthenticationDefaults.AuthorizationEndpoint;
        TokenEndpoint = GitHubAuthenticationDefaults.TokenEndpoint;
        UserInformationEndpoint = GitHubAuthenticationDefaults.UserInformationEndpoint;

        ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
        ClaimActions.MapJsonKey(ClaimTypes.Name, "login");
        ClaimActions.MapJsonKey(ClaimTypes.Email, "email");

        //добавь все scope которые есть у github в этот список иначе не будут возвращаться данные из них в claims 
        //https://docs.github.com/en/developers/apps/building-oauth-apps/scopes-for-oauth-apps
        Scope.Add("user:email");

    }

    public string? EnterpriseDomain { get; set; }
    public string UserEmailsEndpoint { get; set; } = GitHubAuthenticationDefaults.UserEmailsEndpoint;
}
