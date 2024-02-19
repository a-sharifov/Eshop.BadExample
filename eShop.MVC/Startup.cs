using eShop.MVC.AutoMapperProfile;
using eShop.MVC.Protos.Cart;
using eShop.MVC.Protos.Product;
using eShop.MVC.Services.Classes;
using eShop.MVC.Services.Intefaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Logging;

namespace eShop.MVC;

public class Startup
{
    private readonly IConfiguration _configuration;
//  напиши unit тесты на Startup.cs
    public Startup(IConfiguration configuration) =>
        _configuration = configuration;


    public void ConfigureServices(IServiceCollection services)
    {
        IdentityModelEventSource.ShowPII = true;

        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<ProductAutoMapperProfile>();
            cfg.AddProfile<CartAutoMapperProfile>();
        });

        services.AddGrpcClient<DefaultProductService.DefaultProductServiceClient>(configure =>
        {
            configure.Address = new Uri(_configuration["ConnectionStrings:ProductConnection"]!);
        });

        services.AddGrpcClient<DefaultCartService.DefaultCartServiceClient>(configure =>
        {
            configure.Address = new Uri(_configuration["ConnectionStrings:CartConnection"]!);
        });

        services.AddControllersWithViews();

        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICartService, CartService>();
        services.AddScoped<IProductPaginationService, ProductPaginationService>();

        services.AddAuthentication(options =>
        {
            options.DefaultScheme = SD.DefaultScheme;
            options.DefaultChallengeScheme = SD.DefaultChallengeScheme;
        })
        .AddCookie(options =>
        {
            options.Cookie.HttpOnly = true;
            options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
            options.LoginPath = "/Auth/Login";
            options.AccessDeniedPath = "/Auth/AccessDenied";
            options.SlidingExpiration = true;
        })
        .AddOpenIdConnect(SD.DefaultChallengeScheme, options =>
        {
            options.Authority = _configuration["ConnectionStrings:IdentityConnection"];
            options.GetClaimsFromUserInfoEndpoint = true;
            options.ClientId = "eShop.MVC";
            options.ClientSecret = "secret";
            options.ResponseType = "code";
            options.TokenValidationParameters.NameClaimType = "name";
            options.TokenValidationParameters.RoleClaimType = "role";
            options.SaveTokens = true;

            options.ClaimActions.MapJsonKey("role", "role");

            options.Events = new OpenIdConnectEvents()
            {
                OnAccessDenied = context =>
                {
                    context.Response.Redirect("/");
                    context.HandleResponse();
                    return Task.FromResult(0);
                }
            };
        });

        services.AddDistributedMemoryCache();
        services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(10);
        });

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
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseSession();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
        });

    }
}