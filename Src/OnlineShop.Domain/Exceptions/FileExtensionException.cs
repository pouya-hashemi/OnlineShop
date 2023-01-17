using OnlineShop.Domain.Exceptions.BaseExceptions;

namespace OnlineShop.Domain.Exceptions;

public class FileExtensionException:BadRequestException
{
    public FileExtensionException(string expectedExtension) : base($"The file extension should be ' {expectedExtension} ' ")
    {
    }
}