using OnlineShop.Domain.Entities;

namespace OnlineShop.Domain.Interfaces.DomainServiceInterfaces;

public interface IProductManager
{
    Task<Product> CreateProductAsync(string name, string imageUrl, decimal price, int quantity, Category category,CancellationToken cancellationToken=default);
    Task ChangeNameAsync(Product product, string name, CancellationToken cancellationToken = default);
    void ChangeImageUrl(Product product, string imageUrl);
    void ChangePrice(Product product, decimal price);
    void ChangeQuantity(Product product, int quantity);
    void ChangeCategory(Product product, Category category);
}