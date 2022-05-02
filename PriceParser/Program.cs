using Hangfire;
using Hangfire.SqlServer;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using PriceParser.Core;
using PriceParser.Core.Interfaces;
using PriceParser.Core.Interfaces.Data;
using PriceParser.Data;
using PriceParser.Data.Entities;
using PriceParser.DataAccess;
using PriceParser.Domain;
using PriceParser.Domain.CQS;
using PriceParser.Domain.Utils;
using Serilog;
using System.Reflection;
using Twilio;

namespace PriceParser
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration.AddJsonFile("appsettings_private.json");

            builder.Host.UseSerilog((ctx, lc) => lc
                .WriteTo.Console()
                .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day));

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole<Guid>>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddControllersWithViews();

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

            builder.Services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.SlidingExpiration = true;
            });

            //Phone and Email verification services
            TwilioClient.Init(builder.Configuration["IdentitySecrets:SMS_SID"], builder.Configuration["IdentitySecrets:SMS_Token"]);
            builder.Services.Configure<TwilioVerifySettings>(builder.Configuration.GetSection("IdentitySecrets"));
            builder.Services.AddTransient<IEmailSender, EmailSender>();
            builder.Services.AddAuthentication().AddGoogle(googleOptions =>
            {
                googleOptions.ClientId = builder.Configuration["IdentitySecrets:Google:ClientId"];
                googleOptions.ClientSecret = builder.Configuration["IdentitySecrets:Google:ClientSecret"];
            });

            builder.Services.AddAuthorization(builder =>
            {
                builder.AddPolicy("Hangfire", policy => policy.RequireRole(UserRoles.Admin));
            });

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            builder.Services.AddScoped<IRepository<Product>, Repository<Product>>();
            builder.Services.AddScoped<IRepository<MarketSite>, Repository<MarketSite>>();
            builder.Services.AddScoped<IRepository<ProductFromSites>, Repository<ProductFromSites>>();
            builder.Services.AddScoped<IRepository<ProductPrice>, Repository<ProductPrice>>();
            builder.Services.AddScoped<IRepository<UserReview>, Repository<UserReview>>();
            builder.Services.AddScoped<IRepository<Currency>, Repository<Currency>>();
            builder.Services.AddScoped<ICurrencyRatesRepository, CurrencyRatesRepository>();
            builder.Services.AddScoped<IRepository<RefreshToken>, Repository<RefreshToken>>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IProductsService, ProductCQSService>();
            builder.Services.AddScoped<IMarketSitesService, MarketSitesService>();
            builder.Services.AddScoped<IProductsFromSitesService, ProductFromSitesService>();
            builder.Services.AddScoped<IUserReviewsService, UserReviewsService>();
            builder.Services.AddScoped<IProductPricesService, ProductPricesCQSService>();
            builder.Services.AddScoped<IParsingPricesService, ParcingPricesService>();
            builder.Services.AddScoped<ICurrenciesService, CurrenciesService>();

            //to get working auto registering mediatr
            Assembly.Load("PriceParser.CQS");

            builder.Services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());



            // Add Hangfire services.
            builder.Services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireConnection"), new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true
                }));

            // Add the processing server as IHostedService
            builder.Services.AddHangfireServer();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.MapHangfireDashboardWithAuthorizationPolicy("Hangfire");


            //apply migrations on every startup
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    db.Database.Migrate();
                }
                catch (Exception ex)
                {

                    var logger = services.GetRequiredService<Microsoft.Extensions.Logging.ILogger>();
                    logger.LogError(ex, "An error occurred while migrating the database.");
                }

                try
                {
                    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
                    var obj = new UserRoles();
                    foreach (var roleName in UserRoles.RolesList())
                    {
                        await roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
                    }
                }
                catch (Exception ex)
                {

                    var logger = services.GetRequiredService<Microsoft.Extensions.Logging.ILogger>();
                    logger.LogError(ex, "An error occurred while creating roles.");
                }
            }

            app.Run();
        }
    }
}
