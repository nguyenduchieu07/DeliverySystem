using DataAccessLayer.Abstractions.IRepositories;
using DataAccessLayer.Configs;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositoies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceLayer.Abstractions.IRepositories;

namespace DataAccessLayer.DependencyInjections.Extensions
{
    public static class DependencyInjection
    {
        public static void AddIdentityFrameWork(this IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole<Guid>>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
            })
            .AddEntityFrameworkStores<DeliverySytemContext>()
            .AddDefaultTokenProviders();
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
            var geminiConfig = configuration.GetSection("Gemini");
            services.Configure<GeminiConfig>(geminiConfig);
            // Đăng ký generic repository cho tất cả các entity cần thiết
            services.AddScoped(typeof(IBaseRepository<,>), typeof(BaseRepository<,>));
            // Đăng ký repository cụ thể
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IDeliveryRepository, DeliveryRepository>(); // Thêm đăng ký này
            // Đăng ký repository cụ thể cho các entity liên quan đến order
            services.AddScoped<IBaseRepository<Address, Guid>, BaseRepository<Address, Guid>>();
            services.AddScoped<IBaseRepository<Order, Guid>, BaseRepository<Order, Guid>>();
            services.AddScoped<IBaseRepository<OrderItem, Guid>, BaseRepository<OrderItem, Guid>>();
            services.AddScoped<IBaseRepository<Category, Guid>, BaseRepository<Category, Guid>>();
            services.AddScoped<IBaseRepository<Customer, Guid>, BaseRepository<Customer, Guid>>();
            services.AddScoped<IBaseRepository<Store, Guid>, BaseRepository<Store, Guid>>();
            services.AddScoped<IKycRepository, KycRepository>();
            services.AddScoped<IFeedbackRepository, FeedbackRepository>();
            services.AddScoped<IContractRepository, ContractRepository>();
            services.AddScoped<IQuotationRepository, QuotationRepository>();
            services.AddScoped<IBaseRepository<WarehouseSlot, Guid>, BaseRepository<WarehouseSlot, Guid>>();
            services.AddScoped<IBaseRepository<SlotReservation, Guid>, BaseRepository<SlotReservation, Guid>>();
            services.AddScoped<IBaseRepository<Contract, Guid>, BaseRepository<Contract, Guid>>();
            services.AddScoped<IStoreRepository, StoreRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
        }
    }
}