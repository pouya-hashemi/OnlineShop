using OnlineShop.Domain.Exceptions.BaseExceptions;

namespace OnlineShop.Domain.Exceptions;

public class NullOrEmptyException:BadRequestException
{
    public NullOrEmptyException(string propertyName) 
        : base($"{propertyName} is required")
    {
    }
}