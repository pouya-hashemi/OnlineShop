using Microsoft.AspNetCore.Http;

namespace OnlineShop.Domain.Interfaces;

public interface IFileService
{
    Task<string> SaveImageFileAsync(IFormFile file,CancellationToken cancellationToken=default);
    void RemoveFile(string path);
    string ConvertFilePathToFileUrl(string filePath);
}