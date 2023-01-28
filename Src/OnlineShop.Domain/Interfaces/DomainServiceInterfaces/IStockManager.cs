using OnlineShop.Domain.Entities;

namespace OnlineShop.Domain.Interfaces.DomainServiceInterfaces;

public interface IStockManager
{
    Task<bool> ProductHasSellableStockQuantityAsync(long productId, int sellQuantity = 0,
        CancellationToken cancellationToken = default);
}