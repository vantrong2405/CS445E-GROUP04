using BooksStore.DataAccess.Database;
using BooksStore.DataAccess.Repositories;
using BooksStore.DataAccess.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Microsoft.AspNetCore.Identity;
using BooksStore.Utilities;
using Microsoft.AspNetCore.Identity.UI.Services;
using BooksStore.DataAccess.DbInitializer;

namespace BooksStore.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromMinutes(100);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            builder.Services.AddScoped<IDbInitializer, DbInitializer>();

            builder.Services.AddRazorPages();

            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddScoped<IEmailSender, EmailSender>();

            builder.Services.AddDbContext<BooksStoreDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddIdentity<IdentityUser, IdentityRole>
                (options => options.SignIn.RequireConfirmedAccount = false).AddEntityFrameworkStores<BooksStoreDbContext>()
                .AddDefaultTokenProviders();
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/Identity/Account/Login"; //để ứng dụng chuyển hướng đến nếu truy cập chức năng nào đó cần User phải đăng nhập
                options.LogoutPath = $"/Identity/Account/Logout"; //nhấn vào liên kết đăng xuất, họ sẽ được chuyển hướng đến đường dẫn này để tiến hành đăng xuất khỏi hệ thống.
                options.AccessDeniedPath = $"/Identity/Account/AccessDenied"; //chuyển hướng đến nếu User truy cập chức năng nào đó mà không được phân quyền (không có role được phép truy cập)
            });

            builder.Services.AddHttpLogging(logging =>
            {
                logging.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.All;
                logging.RequestHeaders.Add("sec-ch-ua");
                logging.ResponseHeaders.Add("MyResponseHeader");
                logging.MediaTypeOptions.AddText("application/javascript");
                logging.RequestBodyLogLimit = 4096;
                logging.ResponseBodyLogLimit = 4096;
                logging.CombineLogs = true;
            });

            // Serilog
            builder.Host.UseSerilog((context, services, loggerConfiguration) =>
            {
                loggerConfiguration
                    .ReadFrom.Configuration(context.Configuration)
                    .ReadFrom.Services(services); //read out current app's services and make them available to serilog
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
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

            app.UseSession();

            //SeedDatabase();

            app.MapRazorPages();
            app.MapControllerRoute(
                name: "default",
                pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

            app.Run();

            //void SeedDatabase()
            //{
            //    using (var scope = app.Services.CreateScope())
            //    {
            //        var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
            //        dbInitializer.Initialize();
            //    }
            //}
        }
    }
}
