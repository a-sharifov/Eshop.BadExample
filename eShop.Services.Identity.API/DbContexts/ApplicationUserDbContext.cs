using eShop.Services.Identity.API.DbContexts.EntityConfigurations;
using eShop.Services.Identity.API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace eShop.Services.Identity.API.DbContexts;

public class ApplicationUserDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationUserDbContext(DbContextOptions<ApplicationUserDbContext> options)
        : base(options) { }

    public DbSet<ApplicationUser> ApplicationUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new ApplicationUserEntityTypeConfiguration());
    }
}