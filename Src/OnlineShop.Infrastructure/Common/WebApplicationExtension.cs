using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OnlineShop.Domain.Interfaces;
using OnlineShop.Infrastructure.Persistence.SqlServer;

namespace OnlineShop.Infrastructure.Common;

public static class WebApplicationExtension
{
    public static WebApplication ApplyMigrations(this WebApplication app)
    {
        Console.WriteLine("--> Starting Migration...");
        using var scope=app.Services.CreateScope();
        
        var dbContext=scope.ServiceProvider.GetService<IAppDbContext>();
        if (dbContext is null)
        {
            Console.WriteLine("--> Migration failed.");
            Console.WriteLine("--> IAppDbContext Couldn't Initialized");
            return app;
        }

        dbContext.Database.EnsureCreated();
        
        Console.WriteLine("--> Migration Complete...");
        
        return app;
    }
}