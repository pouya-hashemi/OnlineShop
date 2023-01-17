using OnlineShop.Domain.Exceptions.BaseExceptions;

namespace OnlineShop.Domain.Exceptions;

public class FileSizeException:BadRequestException
{
    public FileSizeException(long maxSize) : base($"The maximum size of file is : ' {maxSize} '")
    {
    }
}