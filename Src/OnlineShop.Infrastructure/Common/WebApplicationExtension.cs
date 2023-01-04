using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace OnlineShop.Infrastructure.Common;

public static class WebApplicationExtension
{
    public static WebApplication ApplyMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<DbContext>();
        Console.WriteLine("--> Migrating...");
        db.Database.Migrate();
        Console.WriteLine("--> Migration Done");
        return app;
    }
}