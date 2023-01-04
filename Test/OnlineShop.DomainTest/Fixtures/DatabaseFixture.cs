using System.Data.Common;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Domain.Interfaces;
using OnlineShop.Infrastructure.Persistence.SqlServer;
using Respawn;

namespace OnlineShop.DomainTest.Fixtures;

public class DatabaseFixture: IAsyncLifetime
{
    public IAppDbContext DbContext { get; private set; }
    private DbConnection _dbConnection = default;
    private Respawner _respawner = default;
    
    public DatabaseFixture()
    {
        
    }

    public async Task ResetDatabaseAsync()
    {
        await _respawner.ResetAsync(_dbConnection);
    }
    
    public async Task InitializeAsync()
    {

        await InitializeRespawner();

        var dbOption = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer("Server=.; Database=OnlineShopPouya;User Id=sa; password=symbian;TrustServerCertificate=True")
            .Options;
        
        DbContext = new AppDbContext(dbOption);
    }

    private async Task InitializeRespawner()
    {
        _dbConnection =
            new SqlConnection(
                "Server=.; Database=OnlineShopPouya;User Id=sa; password=symbian;TrustServerCertificate=True");

        await _dbConnection.OpenAsync();
        _respawner = await Respawner.CreateAsync(_dbConnection, new RespawnerOptions()
        {
            DbAdapter = DbAdapter.SqlServer
        });
    }

    public Task DisposeAsync()=>Task.CompletedTask;
}