using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Domain.Exceptions.BaseExceptions;
using OnlineShop.Domain.Interfaces;
using OnlineShop.Domain.Interfaces.DomainServiceInterfaces;

namespace OnlineShop.Application.ApplicationServices.ProductServices.Commands;

public class DeleteProductCommand:IRequest
{
    public long ProductId { get; set; }
}

public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, Unit>
{
    private readonly IAppDbContext _context;
    private readonly IProductManager _productManager;

    public DeleteProductHandler(IAppDbContext context,
        IProductManager productManager)
    {
        _context = context;
        _productManager = productManager;
    }
    public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _context.Products
            .Where(w => w.Id == request.ProductId)
            .FirstOrDefaultAsync(cancellationToken);

        if (product is null)
        {
            throw new NotFoundException(nameof(product));
        }
        
        _context.Products.Remove(product);

        await _context.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;  

    }
}