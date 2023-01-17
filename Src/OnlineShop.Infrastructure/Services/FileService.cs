using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using OnlineShop.Domain.Exceptions;
using OnlineShop.Domain.Interfaces;

namespace OnlineShop.Infrastructure.Services;

public class FileService : IFileService
{
    private readonly string? _baseFilePath;

    public FileService(IConfiguration configuration)
    {
        _baseFilePath = configuration["FileStoringBasePath"];
    }

    public async Task<string> SaveImageFile(IFormFile file)
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
        await file.CopyToAsync(stream);

        return path.Replace(_baseFilePath,"");
    }

    public void RemoveFile(string path)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }
}