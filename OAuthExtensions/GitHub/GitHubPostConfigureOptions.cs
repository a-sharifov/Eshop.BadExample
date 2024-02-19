using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;
using static eShop.Services.Identity.OAuthExtensions.GitHub.GitHubAuthenticationDefaults;

namespace eShop.Services.Identity.OAuthExtensions.GitHub;

public class GitHubPostConfigureOptions : IPostConfigureOptions<GitHubAuthenticationOptions>
{
    public void PostConfigure(
        string? name,
        [NotNull] GitHubAuthenticationOptions options)
    {
        if (!string.IsNullOrWhiteSpace(options.EnterpriseDomain))
        {
            options.AuthorizationEndpoint = CreateUrl(options.EnterpriseDomain, AuthorizationEndpointPath);
            options.TokenEndpoint = CreateUrl(options.EnterpriseDomain, TokenEndpointPath);
            options.UserEmailsEndpoint = CreateUrl(options.EnterpriseDomain, EnterpriseApiPath + UserEmailsEndpointPath);
            options.UserInformationEndpoint = CreateUrl(options.EnterpriseDomain, EnterpriseApiPath + UserInformationEndpointPath);
        }
    }

    private static string CreateUrl(string domain, string path)
    {
        var builder = new UriBuilder(domain)
        {
            Path = path,
            Port = -1,
            Scheme = Uri.UriSchemeHttps,
        };

        return builder.Uri.ToString();
    }
}
