using eShop.Services.Identity.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace eShop.Services.Identity.API.DbContexts.DbInitializers;

public class DbInitializer : IDbInitializer
{
    private readonly ApplicationUserDbContext _dbContext;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public DbInitializer(
               ApplicationUserDbContext dbContext,
               UserManager<ApplicationUser> userManager,
               RoleManager<IdentityRole> roleManager) =>
                (_dbContext, _userManager, _roleManager) = (dbContext, userManager, roleManager);

    public async Task InitializeAsync()
    {
        await _dbContext.Database.MigrateAsync();
        await SeedRolesAsync();
    }

    private async Task SeedRolesAsync()
    {
        var roles = typeof(SD.Role).GetEnumNames();

        foreach (var role in roles)
        {
            if (!await _roleManager.RoleExistsAsync(role))
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }
}