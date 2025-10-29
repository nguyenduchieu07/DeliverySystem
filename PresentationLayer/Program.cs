using DataAccessLayer.DependencyInjections.Extensions;
using DataAccessLayer.Entities;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authentication.Cookies;
using ServiceLayer.Abstractions.IServices;
using ServiceLayer.Extensions;
using ServiceLayer.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDatabaseConfiguration(builder.Configuration); // Đăng ký DbContext
builder.Services.AddIdentityFrameWork(); // Đăng ký Identity
builder.Services.ConfigureRepositories(builder.Configuration);
builder.Services.AddServices(); // Đăng ký Service từ ServiceLayer
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/Denied";
        options.ExpireTimeSpan = TimeSpan.FromHours(2);
        options.SlidingExpiration = true;
    });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
});
builder.Services.AddSingleton<IConverter>(new SynchronizedConverter(new PdfTools()));
// Register EmailSender with SMTP
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("Smtp"));
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddSession();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();

// Quan trọng: UseAuthentication phải trước UseAuthorization
app.UseAuthentication();
app.UseAuthorization();

// Định nghĩa route
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();