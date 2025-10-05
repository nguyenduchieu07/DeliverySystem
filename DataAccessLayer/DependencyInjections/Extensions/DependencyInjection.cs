using DataAccessLayer.Abstractions.IRepositories;
using DataAccessLayer.Configs;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositoies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccessLayer.DependencyInjections.Extensions
{
    public static class DependencyInjection
    {
        public static void AddIdentityFrameWork(this IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole<Guid>>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireDigit = false; // Tùy chỉnh yêu cầu mật khẩu
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.SignIn.RequireConfirmedEmail = false; // Tùy chỉnh đăng nhập
                options.SignIn.RequireConfirmedPhoneNumber = false;
            })
            .AddEntityFrameworkStores<DeliverySytemContext>()
            .AddDefaultTokenProviders();
            // Xóa các dòng sau vì AddIdentity đã đăng ký UserManager và SignInManager
            // services.AddScoped<UserManager<User>>();
            // services.AddScoped<SignInManager<User>>();
        }

        public static void AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DeliverySytemContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });
        }

        public static void ConfigureRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            var cloudinaryConfig = configuration.GetSection("Cloudinary");
            services.Configure<CloudinaryConfig>(cloudinaryConfig);
            services.AddScoped(typeof(IBaseRepository<,>), typeof(BaseRepository<,>));
            services.AddScoped<IUserRepository, UserRepository>();
        }
    }
}