using DataAccessLayer.Entities;
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

            }).AddEntityFrameworkStores<DeliverySytemContext>()
            .AddDefaultTokenProviders();
        }

        public static void AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DeliverySytemContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });
        }
    }
}
