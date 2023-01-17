using Microsoft.Extensions.Configuration;


namespace OnlineShop.ApplicationTest.Common;

public class Utilities
{
    public IConfiguration Configuration { get; set; }
    public Utilities()
    {
        var myConfiguration = new Dictionary<string, string?>
        {
            {"FileStoringBasePath", "C:\\FileArchive\\Tests"},
        };
        
        Configuration= new ConfigurationBuilder()
            .AddInMemoryCollection(myConfiguration)
            .Build();
    }
}