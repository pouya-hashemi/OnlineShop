using OnlineShop.Domain.Entities;

namespace OnlineShop.Domain.Interfaces.DomainServiceInterfaces;

public interface IProductManager
{
    /// <summary>
    /// validates all the inputs and create a valid product
    /// </summary>
    /// <param name="name"></param>
    /// <param name="imageUrl">path of the image</param>
    /// <param name="price"></param>
    /// <param name="quantity"></param>
    /// <param name="category"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>a valid product entity</returns>
    /// <exception cref="AlreadyExistException">name should uniq</exception>
    Task<Product> CreateProductAsync(string name, string imageUrl, decimal price, int quantity, Category category,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// validate and change product name
    /// </summary>
    /// <param name="product"></param>
    /// <param name="name"></param>
    /// <param name="cancellationToken"></param>
    /// <exception cref="ArgumentNullException">product must have value</exception>
    /// <exception cref="AlreadyExistException">name should uniq</exception>
    Task ChangeNameAsync(Product product, string name, CancellationToken cancellationToken = default);

    /// <summary>
    /// validate and change imageUrl of product
    /// </summary>
    /// <param name="product"></param>
    /// <param name="imageUrl"></param>
    /// <exception cref="ArgumentNullException">product must have value</exception>
    void ChangeImageUrl(Product product, string imageUrl);

    /// <summary>
    /// validate and change price property of product
    /// </summary>
    /// <param name="product"></param>
    /// <param name="price"></param>
    /// <exception cref="ArgumentNullException">product must have value</exception>
    void ChangePrice(Product product, decimal price);

    /// <summary>
    /// validate and change quantity of product
    /// </summary>
    /// <param name="product"></param>
    /// <param name="quantity"></param>
    /// <exception cref="ArgumentNullException">product must have value</exception>
    void ChangeQuantity(Product product, int quantity);

    /// <summary>
    /// validate and change category of product
    /// </summary>
    /// <param name="product"></param>
    /// <param name="category"></param>
    /// <exception cref="ArgumentNullException">product must have value</exception>
    void ChangeCategory(Product product, Category category);
}