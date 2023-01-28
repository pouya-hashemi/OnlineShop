using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Domain.Exceptions.BaseExceptions;
using OnlineShop.Domain.Interfaces;

namespace OnlineShop.Application.ApplicationServices.CartServices.Commands;

public class DeleteCartCommand:IRequest
{
    public long CartId { get; set; }
}
public class DeleteCartHandler:IRequestHandler<DeleteCartCommand,Unit>
{
    private readonly IAppDbContext _context;

    public DeleteCartHandler(IAppDbContext context)
    {
        _context = context;
    }
    public async Task<Unit> Handle(DeleteCartCommand request, CancellationToken cancellationToken)
    {
        var cart = await _context.Carts
            .Where(w => w.Id == request.CartId)
            .FirstOrDefaultAsync(cancellationToken);

        if (cart is null)
        {
            throw new NotFoundException(nameof(cart));
        }

        _context.Carts.Remove(cart);
        await _context.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}