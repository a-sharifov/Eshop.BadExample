using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;

namespace eShop.Services.Identity.OAuthExtensions.GitHub;

public static class GitHubAuthenticationExtensions
{
    public static AuthenticationBuilder AddGitHub([NotNull] this AuthenticationBuilder builder)
    {
        return builder.AddGitHub(GitHubAuthenticationDefaults.AuthenticationScheme, options => { });
    }

    public static AuthenticationBuilder AddGitHub(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] Action<GitHubAuthenticationOptions> configuration)
    {
        return builder.AddGitHub(GitHubAuthenticationDefaults.AuthenticationScheme, configuration);
    }

    public static AuthenticationBuilder AddGitHub(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] string scheme,
        [NotNull] Action<GitHubAuthenticationOptions> configuration)
    {
        return builder.AddGitHub(scheme, GitHubAuthenticationDefaults.DisplayName, configuration);
    }

    public static AuthenticationBuilder AddGitHub(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] string scheme,
        [JetBrains.Annotations.CanBeNull] string caption,
        [NotNull] Action<GitHubAuthenticationOptions> configuration)
    {
        builder.Services.TryAddSingleton<IPostConfigureOptions<GitHubAuthenticationOptions>, GitHubPostConfigureOptions>();
        return builder.AddOAuth<GitHubAuthenticationOptions, GitHubAuthenticationHandler>(scheme, caption, configuration);
    }
}
