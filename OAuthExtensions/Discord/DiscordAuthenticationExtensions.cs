using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace eShop.Services.Identity.OAuthExtensions.Discord;

public static class DiscordAuthenticationExtensions
{
    public static AuthenticationBuilder AddDiscord([NotNull] this AuthenticationBuilder builder)
    {
        return builder.AddDiscord(DiscordAuthenticationDefaults.AuthenticationScheme, options => { });
    }

    public static AuthenticationBuilder AddDiscord(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] Action<DiscordAuthenticationOptions> configuration)
    {
        return builder.AddDiscord(DiscordAuthenticationDefaults.AuthenticationScheme, configuration);
    }

    public static AuthenticationBuilder AddDiscord(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] string scheme,
        [NotNull] Action<DiscordAuthenticationOptions> configuration)
    {
        return builder.AddDiscord(scheme, DiscordAuthenticationDefaults.DisplayName, configuration);
    }

    public static AuthenticationBuilder AddDiscord(
        [NotNull] this AuthenticationBuilder builder,
        [NotNull] string scheme,
        [JetBrains.Annotations.CanBeNull] string caption,
        [NotNull] Action<DiscordAuthenticationOptions> configuration)
    {
        return builder.AddOAuth<DiscordAuthenticationOptions, DiscordAuthenticationHandler>(scheme, caption, configuration);
    }
}
