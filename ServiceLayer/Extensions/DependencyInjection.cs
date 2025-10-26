using Microsoft.Extensions.DependencyInjection;
using ServiceLayer.Abstractions.IServices;
using ServiceLayer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Extensions
{
    public static class DependencyInjection
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IKycService, KycService>();
            services.AddScoped<IDashboardService, DashboardService>();
            services.AddScoped<IStoreRegistrationService,StoreRegistrationService>();
            services.AddSingleton<ICloudinaryService, CloudinaryService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IDeliveryService, DeliveryService>();
            services.AddScoped<IWarehouseSlotImportService, WarehouseSlotImportService>();
        }
    }
}
