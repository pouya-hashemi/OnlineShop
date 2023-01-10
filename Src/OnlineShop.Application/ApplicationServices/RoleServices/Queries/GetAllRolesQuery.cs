using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Application.ApplicationServices.RoleServices.Responses;
using OnlineShop.Application.Common;
using OnlineShop.Application.Extensions;
using OnlineShop.Domain.Interfaces;

namespace OnlineShop.Application.ApplicationServices.RoleServices.Queries;

public class GetAllRolesQuery:PaginationRequest,IRequest<PaginationList<RoleDto>>
{
    public string? SearchValue { get; set; }
}
public class GetAllRolesHandler:IRequestHandler<GetAllRolesQuery,PaginationList<RoleDto>>
{
    private readonly IAppDbContext _context;

    public GetAllRolesHandler(IAppDbContext context)
    {
        _context = context;
    }
    public async Task<PaginationList<RoleDto>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Roles
            .AsQueryable();

        if (!String.IsNullOrWhiteSpace(request.SearchValue))
        {
            query = query.Where(w => w.Name.Contains(request.SearchValue));
        }

        var count = await query.CountAsync(cancellationToken);
        var list = await query
            .OrderBy(o => o.Name)
            .AddPaginate(request)
            .Select(s=>new RoleDto()
            {
                Id = s.Id,
                Name = s.Name
            })
            .ToListAsync(cancellationToken);

        return new PaginationList<RoleDto>(list, count, request);

    }
}