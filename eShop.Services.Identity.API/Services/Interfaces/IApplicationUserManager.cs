using eShop.Services.Identity.API.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace eShop.Services.Identity.API.Services.Interfaces
{
    public interface IApplicationUserManager
    {
        Task AddApplicationUserClaimsAsync(ApplicationUser user);
        Task AddIfNotExists(ApplicationUser applicationUser, string provider, IEnumerable<Claim> claims);
        Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role);
        Task<IdentityResult> CreateUserAsync(ApplicationUser user);
        Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password);
        Task<ApplicationUser> CreateUserAsync(string provider, string providerUserId, string role, IEnumerable<Claim> claims);
        Task<ApplicationUser?> FindByEmailAsync(string email);
        Task<ApplicationUser?> FindByUsernameAsync(string username);
        Task<ApplicationUser> FindOrCreateByEmailUserAsync(string provider, string providerUserId, IEnumerable<Claim> claims, string role = "User");
    }
}