using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using System.Security.Claims;
using static eShop.Services.Identity.OAuthExtensions.GitLab.GitLabAuthenticationConstants;

namespace eShop.Services.Identity.OAuthExtensions.GitLab;

public class GitLabAuthenticationOptions : OAuthOptions
{
    public GitLabAuthenticationOptions()
    {
        AuthorizationEndpoint = GitLabAuthenticationDefaults.AuthorizationEndpoint;
        CallbackPath = GitLabAuthenticationDefaults.CallbackPath;
        TokenEndpoint = GitLabAuthenticationDefaults.TokenEndpoint;
        UserInformationEndpoint = GitLabAuthenticationDefaults.UserInformationEndpoint;

        // Available scopes: https://docs.gitlab.com/ee/integration/oauth_provider.html
        Scope.Add("read_user");

        ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
        ClaimActions.MapJsonKey(ClaimTypes.Name, "username");
        ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
        ClaimActions.MapJsonKey(Claims.Name, "name");
        ClaimActions.MapJsonKey(Claims.Avatar, "avatar_url");
        ClaimActions.MapJsonKey(Claims.Url, "web_url");
    }
}
