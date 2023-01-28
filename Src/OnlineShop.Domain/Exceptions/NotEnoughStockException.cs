using OnlineShop.Domain.Exceptions.BaseExceptions;

namespace OnlineShop.Domain.Exceptions;

public class NotEnoughStockException:BadRequestException
{
    public NotEnoughStockException(string productName) : base($"The product ' {productName} ' has no available stock.")
    {
    }
}