using System.Data.Common;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnlineShop.Application.Common;

using OnlineShop.Domain.Common;
using OnlineShop.Domain.Interfaces;
using OnlineShop.Infrastructure.Common;
using OnlineShop.WebApi.Common;
using Respawn;

namespace OnlineShop.InfrastructureTest.Fixtures;

public class ServiceFixture: IAsyncLifetime
{
    public IAppDbContext DbContext { get; private set; }
    private DbConnection _dbConnection = default;
    private Respawner _respawner = default;
    private IConfiguration _configuration;
    public ServiceProvider ServiceProvider { get; private set; }
    private  ServiceCollection _serviceCollection=null;

    public ServiceFixture()
    {
        
    }

    public async Task ResetDatabaseAsync()
    {
        await _respawner.ResetAsync(_dbConnection);
    }
    
    public async Task InitializeAsync()
    {
        

        _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(PopulateAppSetting())
            .Build();

        InitializeServiceCollection();
        
        DbContext = ServiceProvider.GetService<IAppDbContext>();

        await DbContext.Database.EnsureCreatedAsync(); 
        
        await InitializeRespawner();
    }

    private void InitializeServiceCollection()
    {
        _serviceCollection = new ServiceCollection();

        _serviceCollection.AddDomain();
        _serviceCollection.AddApplication(_configuration);
        _serviceCollection.AddInfrastructure(_configuration);

        _serviceCollection.AddWebApi();
        _serviceCollection.AddLogging();

        _serviceCollection.AddSingleton<IConfiguration>(_configuration);
        
        ServiceProvider=_serviceCollection.BuildServiceProvider();
    }
    
    private Dictionary<string, string?> PopulateAppSetting()
    {
        return new Dictionary<string, string?>
        {
            {"FileStoringBasePath", "C:\\FileArchive\\Tests"},
            {"ConnectionStrings:SqlConnection","Server=127.0.0.1; Database=OnlineShop_ApplicationTest;User Id=sa; password=symbian;TrustServerCertificate=True;"}
        };
    }
    
    private async Task InitializeRespawner()
    {
        _dbConnection =
            new SqlConnection(_configuration.GetConnectionString("SqlConnection"));

        await _dbConnection.OpenAsync();
        _respawner = await Respawner.CreateAsync(_dbConnection, new RespawnerOptions()
        {
            DbAdapter = DbAdapter.SqlServer
        });
    }

    public async Task DisposeAsync()=> await DbContext.Database.EnsureDeletedAsync();
}