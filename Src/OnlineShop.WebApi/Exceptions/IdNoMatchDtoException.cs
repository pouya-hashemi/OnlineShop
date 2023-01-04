using OnlineShop.Domain.Exceptions.BaseExceptions;

namespace OnlineShop.WebApi.Exceptions;

public class IdNoMatchDtoException:BadRequestException
{
    public IdNoMatchDtoException() : base("The Id in Url does not match the Id in Body")
    {
    }
}