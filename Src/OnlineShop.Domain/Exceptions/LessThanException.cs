using OnlineShop.Domain.Exceptions.BaseExceptions;

namespace OnlineShop.Domain.Exceptions;

public class LessThanException:BadRequestException
{
    public LessThanException(string propertyName,long compareValue):base($"The value of ' {propertyName} ' can not be less than ' {compareValue} ' .")
    {
        
    }
}