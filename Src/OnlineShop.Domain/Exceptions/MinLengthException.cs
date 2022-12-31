using OnlineShop.Domain.Exceptions.BaseExceptions;

namespace OnlineShop.Domain.Exceptions;

public class MinLengthException : BadRequestException
{
    public MinLengthException(string propertyName, int minLength)
        : base($"Minimum allowed length for {propertyName} is: {minLength}")
    {
    }
}