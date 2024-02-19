using eShop.Services.Identity.API.Extensions;
using eShop.Services.Identity.API.Models;
using eShop.Services.Identity.API.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace eShop.Services.Identity.API.Services.Classes;

public class ApplicationUserManager : IApplicationUserManager
{
    private readonly UserManager<ApplicationUser> _userManager;

    public ApplicationUserManager(UserManager<ApplicationUser> userManager) =>
        _userManager = userManager;


    public async Task AddIfNotExists(ApplicationUser applicationUser, string provider, IEnumerable<Claim> claims)
    {
        if (!applicationUser.Providers.Contains(provider))
        {
            AddIfNotExists(applicationUser, claims);
            await _userManager.UpdateAsync(applicationUser);
        }
    }

    public async Task<ApplicationUser?> FindByEmailAsync(string email) =>
        await _userManager.FindByEmailAsync(email);


    public async Task<ApplicationUser> FindOrCreateByEmailUserAsync(string provider, string providerUserId,
        IEnumerable<Claim> claims, string role = "User")
    {
        var existingUser = await _userManager.FindByEmailAsync(providerUserId);
        if (existingUser != null)
        {
            if (!existingUser.Providers.Contains(provider))
            {
                AddIfNotExists(existingUser, claims);
                await _userManager.UpdateAsync(existingUser);
            }

            return existingUser;
        }

        return await CreateUserAsync(provider, providerUserId, role, claims);
    }

    public async Task<IdentityResult> CreateUserAsync(ApplicationUser user) =>
            await _userManager.CreateAsync(user);

    public async Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password) =>
            await _userManager.CreateAsync(user, password);

    public async Task<ApplicationUser> CreateUserAsync(string provider, string providerUserId, string role,
    IEnumerable<Claim> claims)
    {
        var user = new ApplicationUser(claims);

        var createResult = await CreateUserAsync(user);

        if (!createResult.Succeeded)
        {
            throw new Exception($"error create user: {string.Join(", ", createResult.Errors)}");
        }

        var addLoginResult =
            await _userManager.AddLoginAsync(user, new UserLoginInfo(provider, providerUserId, provider));

        if (!addLoginResult.Succeeded)
        {
            throw new Exception($"error sign in user : {string.Join(", ", addLoginResult.Errors)}");
        }

        await AddToRoleAsync(user, role);
        await AddApplicationUserClaimsAsync(user);

        return user;
    }

    public async Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role) =>
        await _userManager.AddToRoleAsync(user, role);

    public async Task<ApplicationUser?> FindByUsernameAsync(string username) =>
        await _userManager.FindByNameAsync(username);

    private void AddIfNotExists(ApplicationUser applicationUser, IEnumerable<Claim> claims)
    {
        applicationUser.UserName = applicationUser.UserName ?? claims.FirstOrDefaultValue(ClaimTypes.GivenName) ??
            claims.FirstOrDefaultValue(ClaimTypes.Email);
        applicationUser.FirstName = applicationUser.FirstName ?? claims.FirstOrDefaultValue(ClaimTypes.Surname);
        applicationUser.LastName = applicationUser.LastName ?? claims.FirstOrDefaultValue(ClaimTypes.GivenName);
        applicationUser.Email = applicationUser.Email ?? claims.FirstOrDefaultValue(ClaimTypes.Email);

        var provider = claims.FirstOrDefaultValue(ClaimTypes.AuthenticationMethod);
        if (!applicationUser.Providers.Contains(provider!))
        {
            applicationUser.Providers.Add(provider!);
        }
    }

    /// <summary>
    /// standart claims for application user
    /// add claims to user
    /// Name, Email, GivenName, Surname, Role
    /// </summary>
    public async Task AddApplicationUserClaimsAsync(ApplicationUser user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Surname, user.FirstName),
            new Claim(ClaimTypes.GivenName, user.LastName),
        };

        await _userManager.AddClaimsAsync(user, claims);
    }

}