using Microsoft.EntityFrameworkCore;
using OnlineShop.Domain.Entities;
using OnlineShop.Domain.Exceptions;
using OnlineShop.Domain.Interfaces;
using OnlineShop.Domain.Interfaces.DomainServiceInterfaces;

namespace OnlineShop.Domain.DomainServices;

public class ProductManager:IProductManager
{
    private readonly IAppDbContext _context;

    public ProductManager(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Product> CreateProductAsync(string name, string imageUrl, decimal price, int quantity, Category category,CancellationToken cancellationToken=default)
    {
        if (await ProductNameExistAsync(name,cancellationToken:cancellationToken))
        {
            throw new AlreadyExistException(nameof(name), name);
        }

        return new Product(name, imageUrl, price, quantity, category);
    }

    private async Task<bool> ProductNameExistAsync(string name,long? id=null,CancellationToken cancellationToken=default)
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

    public async Task ChangeNameAsync(Product product,string name,CancellationToken cancellationToken=default)
    {
        if (product is null)
        {
            throw new ArgumentNullException(nameof(product));
        }
        
        if (await ProductNameExistAsync(name,product.Id,cancellationToken))
        {
            throw new AlreadyExistException(nameof(name), name);
        }
        
        product.SetName(name);
        
    }
    public void ChangeImageUrl(Product product,string imageUrl)
    {
        if (product is null)
        {
            throw new ArgumentNullException(nameof(product));
        }
        
        product.SetImageUrl(imageUrl);
        
    }

    public void ChangePrice(Product product,decimal price)
    {
        if (product is null)
        {
            throw new ArgumentNullException(nameof(product));
        }
        
        product.SetPrice(price);
    }
    public void ChangeQuantity(Product product,int quantity)
    {
        if (product is null)
        {
            throw new ArgumentNullException(nameof(product));
        }
        
        product.SetQuantity(quantity);
    }
    public void ChangeCategory(Product product,Category category)
    {
        if (product is null)
        {
            throw new ArgumentNullException(nameof(product));
        }
        
        product.SetCategory(category);
    }
    
}