using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Domain.Exceptions.BaseExceptions;
using OnlineShop.Domain.Interfaces;
using OnlineShop.Domain.Interfaces.DomainServiceInterfaces;

namespace OnlineShop.Application.ApplicationServices.RoleServices.Commands;

public class UpdateRoleCommand : IRequest
{
    public long RoleId { get; set; }
    public string Name { get; set; }
}

public class UpdateRoleHandler : IRequestHandler<UpdateRoleCommand, Unit>
{
    private readonly IAppDbContext _context;
    private readonly IRoleManager _roleManager;

    public UpdateRoleHandler(IAppDbContext context,
        IRoleManager roleManager)
    {
        _context = context;
        _roleManager = roleManager;
    }

    public async Task<Unit> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await _context.Roles
            .FirstOrDefaultAsync(f => f.Id == request.RoleId, cancellationToken);
        
        if (role is null)
        {
            throw new NotFoundException(nameof(role));
        }

        await _roleManager.ChangeNameAsync(role, request.Name);

        await _context.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}