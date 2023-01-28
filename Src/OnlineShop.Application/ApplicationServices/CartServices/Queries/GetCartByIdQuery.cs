using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Application.ApplicationServices.CartServices.Responses;
using OnlineShop.Domain.Exceptions.BaseExceptions;
using OnlineShop.Domain.Interfaces;

namespace OnlineShop.Application.ApplicationServices.CartServices.Queries;

public class GetCartByIdQuery : IRequest<CartDto>
{
    public long CartId { get; set; }
}

public class GetCartByIdHandler : IRequestHandler<GetCartByIdQuery, CartDto>
{
    private readonly IAppDbContext _context;

    public GetCartByIdHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<CartDto> Handle(GetCartByIdQuery request, CancellationToken cancellationToken)
    {
        var cart = await _context.Carts
            .Where(w => w.Id == request.CartId)
            .Select(s => new CartDto()
            {
                Id = s.Id,
                Price = s.Price,
                Quantity = s.Quantity,
                CategoryId = s.Product.CategoryId,
                CategoryName = s.Product.Category.Name,
                ProductId = s.ProductId,
                ProductName = s.Product.Name,
                ImageUrl = s.Product.ImageUrl,
                CreatedDateTime = s.CreatedDateTime,
                CreatedUserId = s.CreatedUserId,
                ModifiedDateTime = s.ModifiedDateTime,
                ModifiedUserId = s.ModifiedUserId
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (cart is null)
        {
            throw new NotFoundException(nameof(cart));
        }
        

        return cart;
    }
}