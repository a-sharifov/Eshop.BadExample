using eShop.ServicesCart.API.AutoMapperProfiles;
using eShop.ServicesCart.API.Databases.Configurations;
using eShop.ServicesCart.API.Databases.DbInitializers;
using eShop.ServicesCart.API.Protos.Coupon;
using eShop.ServicesCart.API.Protos.Product;
using eShop.ServicesCart.API.Repositories.Classes;
using eShop.ServicesCart.API.Repositories.Interfaces;
using eShop.ServicesCart.API.Services;
using eShop.ServicesCart.API.Validations;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using MongoDB.Driver;
using StackExchange.Redis;

namespace eShop.ServicesCart.API;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration) =>
        _configuration = configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        services.Configure<DatabaseSettings>(_configuration.GetSection("MongoDatabase:CartSettings"));

        services.AddSingleton<IConnectionMultiplexer>(
            ConnectionMultiplexer.Connect(_configuration["RedisDatabase:ConnectionString"]!));

        services.AddSingleton<IMongoClient>(s =>
            new MongoClient(_configuration["MongoDatabase:ConnectionString"]));

        services.AddValidatorsFromAssemblyContaining<CheckoutHeaderValidator>();

        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<CartAutoMapperProfile>();
            cfg.AddProfile<ProductAutoMapperProfile>();
            cfg.AddProfile<CouponAutoMapperProfile>();
        });

        services.AddGrpcClient<DefaultProductService.DefaultProductServiceClient>(configure =>
        {
            configure.Address = new Uri(_configuration["ConnectionStrings:ProductConnection"]!);
        });

        services.AddGrpcClient<DefaultCouponService.DefaultCouponServiceClient>(configure =>
        {
            configure.Address = new Uri(_configuration["ConnectionStrings:CouponConnection"]!);
        });

        services.AddAuthorization();
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        });

        services.AddScoped<ICartRedisRepository, CartRedisRepository>();
        services.AddScoped<ICartRepository, CartRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
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

        services.AddGrpc();

        services.AddTransient<IDbInitializer, DbInitializer>();

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
            endpoints.MapGrpcService<CartService>();
        });
    }
}