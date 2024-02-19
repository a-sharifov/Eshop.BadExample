using eShop.Services.Identity.API.DbContexts;
using eShop.Services.Identity.API.DbContexts.DbInitializers;
using eShop.Services.Identity.API.Models;
using eShop.Services.Identity.API.Services.Classes;
using eShop.Services.Identity.API.Services.Interfaces;
using eShop.Services.Identity.OAuthExtensions.Discord;
using eShop.Services.Identity.OAuthExtensions.GitHub;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace eShop.Services.Identity.API;

public class Startup
{
    private readonly IConfiguration _сonfiguration;

    public Startup(IConfiguration configuration) =>
        _сonfiguration = configuration;

    public void ConfigureServices(IServiceCollection services)
    {

        services.AddControllersWithViews();

        services.AddDbContext<ApplicationUserDbContext>(options =>
        options.UseSqlServer(_сonfiguration["ConnectionStrings:DefaultConnection"]));

        services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationUserDbContext>()
            .AddDefaultTokenProviders();

        services.AddTransient<IApplicationUserManager, ApplicationUserManager>();

        var builder = services.AddIdentityServer(options =>
        {
            options.Events.RaiseErrorEvents = true;
            options.Events.RaiseInformationEvents = true;
            options.Events.RaiseFailureEvents = true;
            options.Events.RaiseSuccessEvents = true;
            options.EmitStaticAudienceClaim = true;
        })
            .AddInMemoryIdentityResources(SD.IdentityResources)
            .AddInMemoryApiScopes(SD.ApiScopes)
            .AddInMemoryClients(SD.Clients)
            .AddAspNetIdentity<ApplicationUser>();

        services.AddAuthentication(SD.DefaultScheme)
                .AddCookie(SD.DefaultScheme, options =>
                {
                    options.Cookie.Name = "identity";
                    options.ExpireTimeSpan = TimeSpan.FromHours(1);
                    options.LoginPath = "/Account/Login";
                    options.AccessDeniedPath = "/Account/AccessDenied";
                })
                .AddGoogle("Google", options =>
                {
                    options.ClientId = _сonfiguration["GoogleConfiguration:ClientId"]!;
                    options.ClientSecret = _сonfiguration["GoogleConfiguration:ClientSecret"]!;
                    options.SignInScheme = SD.DefaultScheme;
                })
                .AddGitLab("GitLab", options =>
                {
                    options.ClientId = _сonfiguration["GitLabConfiguration:ClientId"]!;
                    options.ClientSecret = _сonfiguration["GitLabConfiguration:ClientSecret"]!;
                    options.SignInScheme = SD.DefaultScheme;
                })
                .AddDiscord("Discord", options =>
                {
                    options.ClientId = _сonfiguration["DiscordConfiguration:ClientId"]!;
                    options.ClientSecret = _сonfiguration["DiscordConfiguration:ClientSecret"]!;
                    options.SignInScheme = SD.DefaultScheme;
                });


        builder.AddDeveloperSigningCredential()
               .AddProfileService<ProfileService>();

        services.AddRazorPages();

        // initialize database
        services.AddTransient<IDbInitializer, DbInitializer>();

        var initializer = services.BuildServiceProvider().GetService<IDbInitializer>();
        initializer.InitializeAsync().GetAwaiter().GetResult();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {

        if (!env.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();
        app.UseIdentityServer();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapRazorPages()
                     .RequireAuthorization();

            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        });
    }
}