using eShop.Services.Identity.OAuthExtensions.GitLab;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace eShop.Services.Identity.OAuthExtensions.GitHub;


public static class GitLabAuthenticationExtensions
{
    public static AuthenticationBuilder AddGitLab([NotNull] this AuthenticationBuilder builder)
    {
        return builder.AddGitLab(GitLabAuthenticationDefaults.AuthenticationScheme, _ => { });
    }

    public static AuthenticationBuilder AddGitLab(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] Action<GitLabAuthenticationOptions> configuration)
    {
        return builder.AddGitLab(GitLabAuthenticationDefaults.AuthenticationScheme, configuration);
    }

    public static AuthenticationBuilder AddGitLab(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] string scheme,
        [NotNull] Action<GitLabAuthenticationOptions> configuration)
    {
        return builder.AddGitLab(scheme, GitLabAuthenticationDefaults.DisplayName, configuration);
    }

    public static AuthenticationBuilder AddGitLab(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] string scheme,
        [NotNull] string caption,
        [NotNull] Action<GitLabAuthenticationOptions> configuration)
    {
        return builder.AddOAuth<GitLabAuthenticationOptions, GitLabAuthenticationHandler>(scheme, caption, configuration);
    }
}
