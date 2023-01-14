using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Application.ApplicationServices.CategoryServices.Responses;
using OnlineShop.Application.Common;
using OnlineShop.Application.Extensions;
using OnlineShop.Domain.Interfaces;

namespace OnlineShop.Application.ApplicationServices.CategoryServices.Queries;

public class GetAllCategoryQuery:PaginationRequest,IRequest<PaginationList<CategoryDto>>
{
    public string? SearchValue { get; set; }
}
public class GetAllCategoryHandler:IRequestHandler<GetAllCategoryQuery,PaginationList<CategoryDto>>
{
    private readonly IAppDbContext _context;

    public GetAllCategoryHandler(IAppDbContext context)
    {
        _context = context;
    }
    public async Task<PaginationList<CategoryDto>> Handle(GetAllCategoryQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Categories
            .AsNoTracking()
            .AsQueryable();

        if (!String.IsNullOrWhiteSpace(request.SearchValue))
        {
            query = query.Where(w => w.Name.Contains(request.SearchValue));
        }

        var list = await query
            .OrderBy(o => o.Name)
            .AddPaginate(request)
            .Select(s=>new CategoryDto()
            {
                Id = s.Id,
                Name = s.Name,
                CreatedDateTime = s.CreatedDateTime,
                CreatedUserId = s.CreatedUserId,
                ModifiedDateTime = s.ModifiedDateTime,
                ModifiedUserId = s.ModifiedUserId
            })
            .ToListAsync(cancellationToken);

        return new PaginationList<CategoryDto>(list, await query.CountAsync(cancellationToken), request);
    }
}