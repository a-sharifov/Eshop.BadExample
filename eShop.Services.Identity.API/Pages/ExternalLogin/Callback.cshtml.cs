using Duende.IdentityServer;
using Duende.IdentityServer.Events;
using Duende.IdentityServer.Services;
using eShop.Services.Identity.API;
using eShop.Services.Identity.API.Extensions;
using eShop.Services.Identity.API.Services.Interfaces;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace eShop.Pages.ExternalLogin;

[AllowAnonymous]
[SecurityHeaders]
public class Callback : PageModel
{
    private readonly IApplicationUserManager _applicationUserManager;
    private readonly IIdentityServerInteractionService _interaction;
    private readonly IEventService _events;

    public Callback(
        IApplicationUserManager applicationUserManager,
        IIdentityServerInteractionService interaction,
        IEventService events) =>
            (_applicationUserManager, _interaction, _events) = (applicationUserManager, interaction, events);


    public async Task<IActionResult> OnGet()
    {
        var result = await HttpContext.AuthenticateAsync(SD.DefaultScheme);
        if (!result.Succeeded)
        {
            throw new Exception("error autentification");
        }

        var externalUser = result.Principal;
        var userEmail = externalUser.FindFirstMailValue();
        var provider = result.Properties.Items["scheme"];

        var user = await _applicationUserManager.FindOrCreateByEmailUserAsync(provider, userEmail, externalUser.Claims);

        var additionalLocalClaims = new List<Claim>();
        var localSignInProps = new AuthenticationProperties();
        CaptureExternalLoginContext(result, additionalLocalClaims, localSignInProps);

        var identityServerUser = new IdentityServerUser(user.Id.ToString())
        {
            DisplayName = user.UserName,
            IdentityProvider = provider,
            AdditionalClaims = additionalLocalClaims
        };

        await HttpContext.SignInAsync(identityServerUser, localSignInProps);
        await HttpContext.SignOutAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);

        var returnUrl = result.Properties.Items["returnUrl"] ?? "~/";

        var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
        await _events.RaiseAsync(new UserLoginSuccessEvent(provider, userEmail, user.Id.ToString(), user.UserName, true, context?.Client?.ClientId));

        if (context != null && context.IsNativeClient())
        {
            return this.LoadingPage(returnUrl);
        }

        return Redirect(returnUrl);
    }

    // if the external login is OIDC-based, there are certain things we need to preserve to make logout work
    // this will be different for WS-Fed, SAML2p or other protocols
    private void CaptureExternalLoginContext(AuthenticateResult externalResult, List<Claim> localClaims, AuthenticationProperties localSignInProps)
    {
        var sid = externalResult.Principal.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.SessionId);
        if (sid != null)
        {
            localClaims.Add(new Claim(JwtClaimTypes.SessionId, sid.Value));
        }

        var idToken = externalResult.Properties.GetTokenValue("id_token");
        if (idToken != null)
        {
            localSignInProps.StoreTokens(new[] { new AuthenticationToken { Name = "id_token", Value = idToken } });
        }
    }
}