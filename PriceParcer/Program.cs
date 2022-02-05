using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PriceParcer;
using PriceParcer.Core;
using PriceParcer.Core.Interfaces;
using PriceParcer.Data;
using PriceParcer.DataAccess;
using PriceParcer.Domain;

namespace PriceParcer
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddControllersWithViews();

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            builder.Services.AddScoped<IRepository<Product>, Repository<Product>>();
            builder.Services.AddScoped<IRepository<MarketSite>, Repository<MarketSite>>();
            builder.Services.AddScoped<IRepository<ProductFromSites>, Repository<ProductFromSites>>();
            builder.Services.AddScoped<IRepository<ProductPrice>, Repository<ProductPrice>>();
            builder.Services.AddScoped<IRepository<UserReview>, Repository<UserReview>>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IProductsService, ProductService>();
            builder.Services.AddScoped<IMarketSitesService, MarketSitesService>();
            builder.Services.AddScoped<IProductsFromSitesService, ProductFromSitesService>();
            builder.Services.AddScoped<IUserReviewsService, UserReviewsService>();




            var app = builder.Build();

            //apply migrations on every startup
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                //try
                //{
                    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    db.Database.Migrate();
                //}
                //catch (Exception ex)
                //{
                    //TODO not working now
                    //var logger = services.GetRequiredService<ILogger>();
                    //logger.LogError(ex, "An error occurred while migrating the database.");
                //}

            }

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

            app.Run();
        }
    }
}
