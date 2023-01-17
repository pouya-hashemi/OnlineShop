using Microsoft.Extensions.Configuration;

namespace OnlineShop.TestShareContent.Common;

public class Utilities
{
    public IConfiguration Configuration { get; set; }
    
    
    public Utilities()
    {
        Configuration= new ConfigurationBuilder()
            .AddInMemoryCollection(PopulateAppSetting())
            .Build();
    }

    private Dictionary<string, string?> PopulateAppSetting()
    {
        return  new Dictionary<string, string?>
        {
            {"FileStoringBasePath", "C:\\FileArchive\\Tests"},
        };
    }
}