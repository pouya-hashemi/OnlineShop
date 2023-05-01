using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Application.ApplicationServices.CartServices.Responses;
using OnlineShop.Application.Common;
using OnlineShop.Application.Extensions;
using OnlineShop.Domain.Entities;
using OnlineShop.Domain.Interfaces;

namespace OnlineShop.Application.ApplicationServices.CartServices.Queries;

public class GetCartsByTokenQuery:PaginationRequest,IRequest<PaginationList<CartDto>>
{
    public string Token { get; set; }
}
public class GetCartsByTokenHandler:IRequestHandler<GetCartsByTokenQuery,PaginationList<CartDto>>
{
    private readonly IAppDbContext _context;

    public GetCartsByTokenHandler(IAppDbContext context)
    {
        _context = context;
    }
    public async Task<PaginationList<CartDto>> Handle(GetCartsByTokenQuery request, CancellationToken cancellationToken)
    {
        var tokenGuid = Guid.Parse(request.Token);
        var query = _context.Carts
            .Where(w => w.Token == tokenGuid)
            .AsQueryable();

        var list = await query
            .OrderBy(o => o.Product.Name)
            .AddPaginate(request)
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
            .ToListAsync(cancellationToken);

        return new PaginationList<CartDto>(list, await query.CountAsync(cancellationToken), request);
    }
}