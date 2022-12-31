using System.Net;

namespace OnlineShop.Domain.Exceptions.BaseExceptions;

public abstract class AppExceptionBase:Exception
{
     public HttpStatusCode StatusCode { get;private set; }
    public AppExceptionBase(string message,HttpStatusCode statusCode):base(message)
    {
        StatusCode = statusCode;
    }
}