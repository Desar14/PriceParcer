using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PriceParser.Core;
using PriceParser.Core.Interfaces;
using PriceParser.Core.Interfaces.Data;
using PriceParser.Data;
using PriceParser.Data.Entities;
using PriceParser.DataAccess;
using PriceParser.Domain;
using Serilog;
using System.Reflection;
using System.Text;

namespace PriceParser.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Configuration.AddJsonFile("appsettings_private.json");

            builder.Host.UseSerilog((ctx, lc) => lc
                .WriteTo.Console()
                .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day));

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            

            builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole<Guid>>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;
            });

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })

            // Adding Jwt Bearer  
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["JWT:ValidAudience"],
                    ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"])),
                    ValidateLifetime = true
                };
            });

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            builder.Services.AddScoped<IRepository<Product>, Repository<Product>>();
            builder.Services.AddScoped<IRepository<MarketSite>, Repository<MarketSite>>();
            builder.Services.AddScoped<IRepository<ProductFromSites>, Repository<ProductFromSites>>();
            builder.Services.AddScoped<IRepository<ProductPrice>, Repository<ProductPrice>>();
            builder.Services.AddScoped<IRepository<UserReview>, Repository<UserReview>>();
            builder.Services.AddScoped<IRepository<Currency>, Repository<Currency>>();
            builder.Services.AddScoped<ICurrencyRatesRepository, CurrencyRatesRepository>();
            builder.Services.AddScoped<IRepository<RefreshToken>, Repository<RefreshToken>>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IProductsService, ProductService>();
            builder.Services.AddScoped<IMarketSitesService, MarketSitesService>();
            builder.Services.AddScoped<IProductsFromSitesService, ProductFromSitesService>();
            builder.Services.AddScoped<IUserReviewsService, UserReviewsService>();
            builder.Services.AddScoped<IProductPricesService, ProductPricesService>();
            builder.Services.AddScoped<ICurrenciesService, CurrenciesService>();
            builder.Services.AddScoped<IUserJWTService, UserJWTService>();

            builder.Services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());

            var app = builder.Build();

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                           ForwardedHeaders.XForwardedProto
            });

            var origins = builder.Configuration.GetSection("CorsOrigins").Get<List<string>>().ToArray();

            app.UseCors(corsBuilder => corsBuilder
                .AllowAnyHeader()
                .AllowAnyMethod()
                .SetIsOriginAllowedToAllowWildcardSubdomains()
                .WithOrigins(builder.Configuration.GetSection("CorsOrigins").Get<List<string>>().ToArray()));

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
