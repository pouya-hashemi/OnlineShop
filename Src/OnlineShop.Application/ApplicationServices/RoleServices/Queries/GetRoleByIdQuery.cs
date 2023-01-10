using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Application.ApplicationServices.RoleServices.Responses;
using OnlineShop.Domain.Exceptions.BaseExceptions;
using OnlineShop.Domain.Interfaces;

namespace OnlineShop.Application.ApplicationServices.RoleServices.Queries;

public class GetRoleByIdQuery:IRequest<RoleDto>
{
    public int RoleId { get; set; }
}
public class GetRoleByIdHandler:IRequestHandler<GetRoleByIdQuery,RoleDto>
{
    private readonly IAppDbContext _context;

    public GetRoleByIdHandler(IAppDbContext context)
    {
        _context = context;
    }
    public async Task<RoleDto> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
    {
        var role = await _context.Roles
            .FirstOrDefaultAsync(f => f.Id == request.RoleId, cancellationToken);

        if (role is null)
        {
            throw new NotFoundException(nameof(role));
        }

        return role.Adapt<RoleDto>();
    }
}