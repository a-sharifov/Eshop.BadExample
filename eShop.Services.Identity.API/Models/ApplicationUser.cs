using eShop.Services.Identity.API.Extensions;
using eShop.Services.Identity.API.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace eShop.Services.Identity.API.Models;

public class ApplicationUser : IdentityUser
{

    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public readonly List<string> Providers = new();

    public ApplicationUser() { }

    public ApplicationUser(RegisterViewModel viewModel)
    {
        UserName = viewModel.UserName;
        FirstName = viewModel.FirstName;
        LastName = viewModel.LastName;
        Email = viewModel.Email;
    }

    public ApplicationUser(IEnumerable<Claim> claims)
    {
        UserName = claims.FirstOrDefaultValue(ClaimTypes.GivenName) ??
            claims.FirstOrDefaultValue(ClaimTypes.Email);
        FirstName = claims.FirstOrDefaultValue(ClaimTypes.Surname);
        LastName = claims.FirstOrDefaultValue(ClaimTypes.GivenName);
        Email = claims.FirstOrDefaultValue(ClaimTypes.Email);
    }
}