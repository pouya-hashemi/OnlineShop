using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnlineShop.Domain.DomainServices;
using OnlineShop.Domain.Entities;
using OnlineShop.Domain.Interfaces;
using OnlineShop.Domain.Interfaces.DomainServiceInterfaces;
using OnlineShop.Infrastructure.Persistence.SqlServer;
using OnlineShop.Infrastructure.Services;

namespace OnlineShop.Infrastructure.Common;

public static class InfrastructureDependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddDbContext<AppDbContext>(opt =>
        {
            opt.UseSqlServer(configuration.GetConnectionString("SqlConnection"));
        });

        services.AddIdentity<User, Role>(options => { })
            .AddEntityFrameworkStores<AppDbContext>();
        
        services.AddScoped<IAppDbContext, AppDbContext>();
        services.AddTransient<IFileService, FileService>();





        return services;
    }
}