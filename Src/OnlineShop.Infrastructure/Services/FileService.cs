using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using OnlineShop.Domain.Exceptions;
using OnlineShop.Domain.Interfaces;

namespace OnlineShop.Infrastructure.Services;

public class FileService : IFileService
{
    private readonly string? _baseFilePath;
    private readonly string? _baseFileUrl;

    public FileService(IConfiguration configuration)
    {
        _baseFilePath = configuration["FileStoringBasePath"];
        _baseFileUrl = configuration["FileDownloadBaseUrl"];
    }

    public async Task<string> SaveImageFileAsync(IFormFile file, CancellationToken cancellationToken = default)
    {
        var acceptedExtensions = new string[] {".jpg", ".jpeg"};
        var maxImageSize = 5000000;

        var fileExtension = Path.GetExtension(file.FileName);

        if (_baseFilePath is null)
        {
            throw new ArgumentNullException("Base file path in app setting");
        }

        if (!acceptedExtensions.Contains(fileExtension))
        {
            throw new FileExtensionException(String.Join(',', acceptedExtensions));
        }

        if (file.Length == 0)
        {
            throw new NullOrEmptyException("image");
        }

        if (file.Length > maxImageSize)
        {
            throw new FileSizeException(maxImageSize);
        }

        var directory = Path.Combine(_baseFilePath, "Images");
        
        Directory.CreateDirectory(directory);

        var path = Path.Combine(directory, Guid.NewGuid().ToString() + fileExtension);


        await using Stream stream = new FileStream(path, FileMode.Create);
        await file.CopyToAsync(stream,cancellationToken);

        return path.Replace(_baseFilePath,"");
    }

    public void RemoveFile(string path)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

    public string ConvertFilePathToFileUrl(string filePath)
    {
        return _baseFileUrl + filePath.Replace("\\", "/");
    }
}