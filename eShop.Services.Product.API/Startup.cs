using eShop.ServicesProduct.API.AutoMapperProfiles;
using eShop.ServicesProduct.API.Builders.Classes;
using eShop.ServicesProduct.API.Builders.Interfaces;
using eShop.ServicesProduct.API.DbContexts;
using eShop.ServicesProduct.API.DbInitializers;
using eShop.ServicesProduct.API.Extensions;
using eShop.ServicesProduct.API.Repositories.Classes;
using eShop.ServicesProduct.API.Repositories.Interfaces;
using eShop.ServicesProduct.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;

namespace eShop.Services.Product.API;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration) =>
        _configuration = configuration;


    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<ProductDbContext>(options =>
            options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection")));

        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<ProductAutoMapperProfile>();
        });

        services.AddAuthorization();
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        });

        services.AddElasticSearch(_configuration);
        services.AddSingleton<IProductElasticQueryBuilder, ProductElasticQueryBuilder>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IProductElasticRepository, ProductElasticRepository>();

        services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.Authority = _configuration.GetConnectionString("IdentityConnection");
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
            endpoints.MapGrpcService<ProductService>();
        });
    }

}