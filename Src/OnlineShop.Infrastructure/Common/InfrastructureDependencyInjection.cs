using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnlineShop.Domain.Interfaces;
using OnlineShop.Infrastructure.Persistence.SqlServer;
using OnlineShop.Infrastructure.Services;

namespace OnlineShop.Infrastructure.Common;

public static class InfrastructureDependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddDbContext<DbContext>(opt =>
        {
            opt.UseSqlServer(configuration.GetConnectionString("SqlConnection"));
        });

        services.AddScoped<IAppDbContext, AppDbContext>();
        services.AddTransient<IFileService,FileService>();
        
        
        return services;
    }
}