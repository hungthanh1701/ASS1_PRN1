using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Assignment_1.Data;
using Assignment_1.Repositories;
using Assignment_1.Services;

namespace Assignment_1.Configuration
{
    public static class ServiceConfiguration
    {
        public static IServiceCollection AddDatabaseServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Add Entity Framework
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Add Repositories
            services.AddScoped<IRoomRepository, RoomRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<IStaffRepository, StaffRepository>();
            services.AddScoped<IAuditLogRepository, AuditLogRepository>();

            // Add Services
            services.AddScoped<IRoomService, RoomService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<IStaffService, StaffService>();
            services.AddScoped<IAuditLogService, AuditLogService>();

            return services;
        }

        public static IServiceCollection AddSingletonServices(this IServiceCollection services)
        {
            // No singleton services needed for now
            return services;
        }
    }
}
