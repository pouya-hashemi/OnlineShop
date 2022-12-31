using OnlineShop.Domain.Exceptions.BaseExceptions;

namespace OnlineShop.Domain.Exceptions;

public class MaxLengthException : BadRequestException
{
    public MaxLengthException(string propertyName, int maxLength) 
        : base($"Maximum allowed length for {propertyName} is: {maxLength}")
    {
    }
}