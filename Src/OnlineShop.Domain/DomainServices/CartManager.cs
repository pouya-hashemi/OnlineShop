using OnlineShop.Domain.Entities;
using OnlineShop.Domain.Exceptions;
using OnlineShop.Domain.Interfaces;
using OnlineShop.Domain.Interfaces.DomainServiceInterfaces;

namespace OnlineShop.Domain.DomainServices;

public class CartManager:ICartManager
{
    private readonly IAppDbContext _context;
    private readonly IStockManager _stockManager;

    public CartManager(IAppDbContext context,
        IStockManager stockManager)
    {
        _context = context;
        _stockManager = stockManager;
    }

    public async Task<Cart> CreateCartAsync(string token, Product product, int quantity,CancellationToken cancellationToken=default)
    {
        if (product is null)
        {
            throw new ArgumentNullException(nameof(product));
        }
        if (!await _stockManager.ProductHasSellableStockQuantityAsync(product.Id,quantity,cancellationToken))
        {
            throw new NotEnoughStockException(product.Name);
        }
        
        return new Cart(token, product, quantity, product.Price);
    }
    
}