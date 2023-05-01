using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OnlineShop.Application.ApplicationServices.ProductServices.Responses;
using OnlineShop.Application.Common;
using OnlineShop.Application.Extensions;
using OnlineShop.Domain.Interfaces;

namespace OnlineShop.Application.ApplicationServices.ProductServices.Queries;

public class GetAllProductsQuery : PaginationRequest, IRequest<PaginationList<ProductDto>>
{
    public string? SearchValue { get; set; }
}

public class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, PaginationList<ProductDto>>
{
    private readonly IAppDbContext _context;
    private readonly IConfiguration _configuration;

    public GetAllProductsHandler(IAppDbContext context,
        IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<PaginationList<ProductDto>> Handle(GetAllProductsQuery request,
        CancellationToken cancellationToken)
    {
        var query = _context.Products
            .Include(i => i.Category)
            .AsNoTracking()
            .AsQueryable();

        if (!String.IsNullOrWhiteSpace(request.SearchValue))
        {
            query = query.Where(w => w.Name.Contains(request.SearchValue));
        }

        var list = await query
            .OrderBy(o => o.Name)
            .AddPaginate(request)
            .Select(s => new ProductDto()
            {
                Id = s.Id,
                Name = s.Name,
                Price = s.Price,
                Quantity = s.Quantity,
                CategoryId = s.CategoryId,
                CategoryName = s.Category.Name,
                ImageUrl =s.ImageUrl,
                CreatedDateTime = s.CreatedDateTime,
                CreatedUserId = s.CreatedUserId,
                ModifiedDateTime = s.ModifiedDateTime,
                ModifiedUserId = s.ModifiedUserId
            })
            .ToListAsync(cancellationToken);

        var totalCount = await query.CountAsync(cancellationToken);

        return new PaginationList<ProductDto>(list, totalCount, request);
    }
}