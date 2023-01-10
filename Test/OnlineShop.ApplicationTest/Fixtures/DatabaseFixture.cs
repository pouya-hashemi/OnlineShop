using System.Data.Common;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Domain.Interfaces;
using OnlineShop.Infrastructure.Persistence.SqlServer;
using Respawn;

namespace OnlineShop.ApplicationTest.Fixtures;

public class DatabaseFixture: IAsyncLifetime
{
    public IAppDbContext DbContext { get; private set; }
    
    
    private DbConnection _dbConnection = default;
    private Respawner _respawner = default;
    private const string _connectionString = "Server=127.0.0.1; Database=OnlineShop_ApplicationTest;User Id=sa; password=symbian;TrustServerCertificate=True;";

    public DatabaseFixture()
    {
        
    }

    public async Task ResetDatabaseAsync()
    {
        await _respawner.ResetAsync(_dbConnection);
    }
    
    public async Task InitializeAsync()
    {

        

        var dbOption = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer(_connectionString)
            .Options;
        
        DbContext = new AppDbContext(dbOption);

        await DbContext.Database.EnsureCreatedAsync(); 
        
        await InitializeRespawner();
    }

    private async Task InitializeRespawner()
    {
        _dbConnection =
            new SqlConnection(_connectionString);

        await _dbConnection.OpenAsync();
        _respawner = await Respawner.CreateAsync(_dbConnection, new RespawnerOptions()
        {
            DbAdapter = DbAdapter.SqlServer
        });
    }

    public async Task DisposeAsync()=> await DbContext.Database.EnsureDeletedAsync();
}