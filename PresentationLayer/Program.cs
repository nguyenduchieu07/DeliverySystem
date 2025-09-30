using DataAccessLayer.DependencyInjections.Extensions;
using DataAccessLayer.Entities;
using ServiceLayer.Abstractions.IServices;
using ServiceLayer.Extensions;
using ServiceLayer.Services;

namespace PresentationLayer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();
            builder.Services.AddDatabaseConfiguration(builder.Configuration);
            builder.Services.AddIdentityFrameWork();
            builder.Services.ConfigureRepositories();
            builder.Services.AddServices(); // từ ServiceLayer

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
            app.MapControllerRoute(
                name : "areas",
                pattern : "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
