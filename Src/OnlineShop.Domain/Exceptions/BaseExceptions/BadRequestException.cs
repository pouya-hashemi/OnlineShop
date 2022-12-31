using System.Net;

namespace OnlineShop.Domain.Exceptions.BaseExceptions;

public class BadRequestException:AppExceptionBase
{
    public BadRequestException(string message) : base(message, HttpStatusCode.BadRequest)
    {
    }
}