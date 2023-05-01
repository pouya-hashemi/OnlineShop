using Microsoft.EntityFrameworkCore;
using OnlineShop.Domain.Entities;
using OnlineShop.Domain.Exceptions;
using OnlineShop.Domain.Interfaces;
using OnlineShop.Domain.Interfaces.DomainServiceInterfaces;

namespace OnlineShop.Domain.DomainServices;

public class ProductManager : IProductManager
{
    private readonly IAppDbContext _context;
    private readonly IFileService _fileService;

    public ProductManager(IAppDbContext context,
        IFileService fileService)
    {
        _context = context;
        _fileService = fileService;
    }

    /// <summary>
    /// validates all the inputs and create a valid product
    /// </summary>
    /// <param name="name"></param>
    /// <param name="imagePath">path of the image</param>
    /// <param name="price"></param>
    /// <param name="quantity"></param>
    /// <param name="category"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>a valid product entity</returns>
    /// <exception cref="AlreadyExistException">name should uniq</exception>
    public async Task<Product> CreateProductAsync(string name, string imagePath, decimal price, int quantity,
        Category category, CancellationToken cancellationToken = default)
    {
        if (await ProductNameExistAsync(name, cancellationToken: cancellationToken))
        {
            throw new AlreadyExistException(nameof(name), name);
        }

        var imageUrl = _fileService.ConvertFilePathToFileUrl(imagePath);
        return new Product(name, imagePath, price, quantity, category,imageUrl);
    }

    private async Task<bool> ProductNameExistAsync(string name, long? id = null,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Products
            .Where(w => w.Name == name)
            .AsNoTracking()
            .AsQueryable();

        if (id is not null)
        {
            query = query.Where(w => w.Id != id);
        }

        return await query.AnyAsync(cancellationToken);
    }

    /// <summary>
    /// validate and change product name
    /// </summary>
    /// <param name="product"></param>
    /// <param name="name"></param>
    /// <param name="cancellationToken"></param>
    /// <exception cref="ArgumentNullException">product must have value</exception>
    /// <exception cref="AlreadyExistException">name should uniq</exception>
    public async Task ChangeNameAsync(Product product, string name, CancellationToken cancellationToken = default)
    {
        if (product is null)
        {
            throw new ArgumentNullException(nameof(product));
        }

        if (await ProductNameExistAsync(name, product.Id, cancellationToken))
        {
            throw new AlreadyExistException(nameof(name), name);
        }

        product.SetName(name);
    }

    /// <summary>
    /// validate and change imageUrl of product
    /// </summary>
    /// <param name="product"></param>
    /// <param name="imagePath"></param>
    /// <exception cref="ArgumentNullException">product must have value</exception>
    public void ChangeImagePath(Product product, string imagePath)
    {
        if (product is null)
        {
            throw new ArgumentNullException(nameof(product));
        }
        var imageUrl = _fileService.ConvertFilePathToFileUrl(imagePath);
        product.SetImagePath(imagePath);
        product.SetImageUrl(imageUrl);
    }

    /// <summary>
    /// validate and change price property of product
    /// </summary>
    /// <param name="product"></param>
    /// <param name="price"></param>
    /// <exception cref="ArgumentNullException">product must have value</exception>
    public void ChangePrice(Product product, decimal price)
    {
        if (product is null)
        {
            throw new ArgumentNullException(nameof(product));
        }

        product.SetPrice(price);
    }

    /// <summary>
    /// validate and change quantity of product
    /// </summary>
    /// <param name="product"></param>
    /// <param name="quantity"></param>
    /// <exception cref="ArgumentNullException">product must have value</exception>
    public void ChangeQuantity(Product product, int quantity)
    {
        if (product is null)
        {
            throw new ArgumentNullException(nameof(product));
        }

        product.SetQuantity(quantity);
    }

    /// <summary>
    /// validate and change category of product
    /// </summary>
    /// <param name="product"></param>
    /// <param name="category"></param>
    /// <exception cref="ArgumentNullException">product must have value</exception>
    public void ChangeCategory(Product product, Category category)
    {
        if (product is null)
        {
            throw new ArgumentNullException(nameof(product));
        }

        product.SetCategory(category);
    }
}