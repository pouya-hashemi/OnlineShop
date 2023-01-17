using OnlineShop.Domain.Interfaces;
using OnlineShop.Infrastructure.Services;
using OnlineShop.TestShareContent.Common;

namespace OnlineShop.TestShareContent.SharedFixtures;

public class FileServiceFixture:IDisposable
{
    public IFileService FileService { get;private set; }
    private readonly string _baseFilePath ;
    public FileServiceFixture()
    {
        var utilities = new Utilities();
        _baseFilePath = utilities.Configuration["FileStoringBasePath"];
        FileService=new FileService(utilities.Configuration);
        Directory.CreateDirectory(_baseFilePath);
    }
    public void Dispose()
    {
        Directory.Delete(_baseFilePath,true);
    }
}