using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Application.ApplicationServices.CartServices.Responses;
using OnlineShop.Domain.Exceptions.BaseExceptions;
using OnlineShop.Domain.Interfaces;
using OnlineShop.Domain.Interfaces.DomainServiceInterfaces;

namespace OnlineShop.Application.ApplicationServices.CartServices.Commands;

public class CreateCartCommand:IRequest<CartDto>
{
    public string Token { get; set; }
    public long ProductId { get; set; }
    public int Quantity { get; set; }
}
public class CreateCartHandler:IRequestHandler<CreateCartCommand,CartDto>
{
    private readonly IAppDbContext _context;
    private readonly ICartManager _cartManager;

    public CreateCartHandler(IAppDbContext context,
        ICartManager cartManager)
    {
        _context = context;
        _cartManager = cartManager;
    }
    public async Task<CartDto> Handle(CreateCartCommand request, CancellationToken cancellationToken)
    {
        var product = await _context.Products
            .Include(i=>i.Category)
            .Where(w => w.Id == request.ProductId)
            .FirstOrDefaultAsync(cancellationToken);

        if (product is null)
        {
            throw new NotFoundException(nameof(product));
        }

        var cart=await _cartManager.CreateCartAsync(request.Token, product, request.Quantity, cancellationToken);

        _context.Carts.Add(cart);
        await _context.SaveChangesAsync(cancellationToken);

        var cartDto = cart.Adapt<CartDto>();
        cartDto.ProductName = product.Name;
        cartDto.CategoryId = product.CategoryId;
        cartDto.CategoryName = product.Category.Name;
        cartDto.ImageUrl = product.ImageUrl;

        return cartDto;
    }
}