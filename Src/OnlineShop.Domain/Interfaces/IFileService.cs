using Microsoft.AspNetCore.Http;

namespace OnlineShop.Domain.Interfaces;

public interface IFileService
{
    Task<string> SaveImageFile(IFormFile file);
    void RemoveFile(string path);
}