using OnlineShop.Domain.Exceptions.BaseExceptions;

namespace OnlineShop.Domain.Exceptions;

public class AlreadyExistException : BadRequestException
{
    public AlreadyExistException(string propertyName, string propertyValue)
        : base($"{propertyName} \" {propertyValue} \" already exists")
    {
    }
}