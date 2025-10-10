using DataAccessLayer.DependencyInjections.Extensions;
using DataAccessLayer.Entities;
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
// Register EmailSender with SMTP
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("Smtp"));
builder.Services.AddTransient<IEmailSender, EmailSender>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

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