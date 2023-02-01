using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Application.ApplicationServices.UserServices.Responses;
using OnlineShop.Application.Common;
using OnlineShop.Application.Extensions;
using OnlineShop.Domain.Interfaces;

namespace OnlineShop.Application.ApplicationServices.UserServices.Queries;

public class GetAllUsersQuery:PaginationRequest,IRequest<PaginationList<UserDto>>
{
    public string? SearchValue { get; set; }
}
public class GetAllUsersHandler:IRequestHandler<GetAllUsersQuery,PaginationList<UserDto>>
{
    private readonly IAppDbContext _context;

    public GetAllUsersHandler(IAppDbContext context)
    {
        _context = context;
    }
    public async Task<PaginationList<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Users
            .AsNoTracking()
            .AsQueryable();

        if (!String.IsNullOrWhiteSpace(request.SearchValue))
        {
            query = query.Where(w => (w.UserName + " " + w.UserTitle).Contains(request.SearchValue));
        }

        var list = await query
            .OrderBy(o => o.UserName)
            .AddPaginate(request)
            .Select(s=>new UserDto()
            {
                Id = s.Id,
                Username = s.UserName,
                UserTitle = s.UserTitle,
                
                CreatedDateTime = s.CreatedDateTime,
                CreatedUserId = s.CreatedUserId,
                ModifiedDateTime = s.ModifiedDateTime,
                ModifiedUserId = s.ModifiedUserId
            })
            .ToListAsync(cancellationToken);

        var count = await query.CountAsync(cancellationToken);
        return  new PaginationList<UserDto>(list, count, request);
        
    }
}