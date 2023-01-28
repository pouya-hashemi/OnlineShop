using OnlineShop.Domain.Entities;

namespace OnlineShop.Domain.Interfaces.DomainServiceInterfaces;

public interface ICartManager
{
    Task<Cart> CreateCartAsync(string token, Product product, int quantity,
        CancellationToken cancellationToken = default);
}