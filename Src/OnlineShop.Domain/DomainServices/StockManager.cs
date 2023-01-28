using Microsoft.EntityFrameworkCore;
using OnlineShop.Domain.Entities;
using OnlineShop.Domain.Exceptions.BaseExceptions;
using OnlineShop.Domain.Interfaces;
using OnlineShop.Domain.Interfaces.DomainServiceInterfaces;

namespace OnlineShop.Domain.DomainServices;

public class StockManager:IStockManager
{
    private readonly IAppDbContext _context;

    public StockManager(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> ProductHasSellableStockQuantityAsync(long productId,int sellQuantity=0,CancellationToken cancellationToken=default)
    {
        var product = await _context.Products
            .Where(w => w.Id == productId)
            .FirstOrDefaultAsync(cancellationToken);
        if (product is null)
        {
            throw new NotFoundException(nameof(product));
        }

        return product.Quantity >= sellQuantity;
    }
}