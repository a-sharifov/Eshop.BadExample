using eShop.ServicesCoupon.API.AutoMapperProfiles;
using eShop.ServicesCoupon.API.DbContexts;
using eShop.ServicesCoupon.API.DbContexts.DbInitializers;
using eShop.ServicesCoupon.API.Repositories.Classes;
using eShop.ServicesCoupon.API.Repositories.Interfaces;
using eShop.ServicesCoupon.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;

namespace eShop.ServicesCoupon.API;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration) =>
        _configuration = configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddAuthorization();
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        });

        services.AddDbContext<CouponDbContext>(options =>
            options.UseNpgsql(_configuration["ConnectionStrings:DefaultConnection"]));

        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<CouponAutoMapperProfile>();
        });

        services.AddScoped<ICouponRepository, CouponRepository>();

        services.AddAuthentication("Bearer")
          .AddJwtBearer("Bearer", options =>
          {
              options.RequireHttpsMetadata = false;
              options.SaveToken = true;
              options.Authority = _configuration["ConnectionStrings:IdentityConnection"];
              options.TokenValidationParameters = new()
              {
                  ValidateAudience = false,
                  ValidateIssuer = false,
              };
          });

        services.AddScoped<IDbInitializer, DbInitializer>();

        var initializer = services.BuildServiceProvider().GetService<IDbInitializer>();
        initializer.InitializeAsync().GetAwaiter().GetResult();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {

        if (!env.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapGrpcService<CouponService>();
        });
    }
}
