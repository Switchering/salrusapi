using API.Data;
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;


namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration _config)
        {
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IWBService, WBService>();
            services.AddScoped<ISalrusService, SalrusService>();
            services.AddScoped<IWBRepository, WBRepository>();
            services.AddAutoMapper(typeof(AppMappingProfile).Assembly);
            services.AddDbContext<DataContext>(options =>
            {
                //options.UseSqlite(_config.GetConnectionString("DefaultConnection"));
                options.UseNpgsql(_config.GetConnectionString("PostgreConnection"));
                options.EnableSensitiveDataLogging();
            });

            return services;
        }
    }
}