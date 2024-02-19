using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using eShop.Services.Identity.API.Models;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace eShop.Services.Identity.API.Services.Classes;

public class ProfileService : IProfileService
{
    private readonly IUserClaimsPrincipalFactory<ApplicationUser> _claimsFactory;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public ProfileService(
        IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager) =>
        (_claimsFactory, _userManager, _roleManager) = (claimsFactory, userManager, roleManager);


    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        string sub = context.Subject.GetSubjectId();
        var user = await _userManager.FindByIdAsync(sub);
        var principal = await _claimsFactory.CreateAsync(user);

        var claims = principal.Claims.ToList();
        claims = claims.Where(claim => context.RequestedClaimTypes
                       .Contains(claim.Type))
                       .ToList();

        claims.Add(new Claim(ClaimTypes.Name, user.UserName));
        claims.Add(new Claim(ClaimTypes.Surname, user.FirstName));
        claims.Add(new Claim(ClaimTypes.GivenName, user.LastName));

        context.IssuedClaims = claims;
        if (!_userManager.SupportsUserRole)
        {
            return;
        }

        IList<string> roles = await _userManager.GetRolesAsync(user);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
            if (_roleManager.SupportsRoleClaims)
            {
                var roleEntity = await _roleManager.FindByNameAsync(role);
                if (roleEntity != null)
                {
                    claims.AddRange(await _roleManager.GetClaimsAsync(roleEntity));
                }
            }
        }
    }

    public async Task IsActiveAsync(IsActiveContext context)
    {
        string sub = context.Subject.GetSubjectId();
        var user = await _userManager.FindByIdAsync(sub);
        context.IsActive = user != null;
    }
}