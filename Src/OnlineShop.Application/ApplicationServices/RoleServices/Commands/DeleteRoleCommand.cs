using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Domain.Exceptions.BaseExceptions;
using OnlineShop.Domain.Interfaces;
using OnlineShop.Domain.Interfaces.DomainServiceInterfaces;

namespace OnlineShop.Application.ApplicationServices.RoleServices.Commands;

public class DeleteRoleCommand:IRequest
{
    public long RoleId { get; set; }
}
public class DeleteRoleHandler:IRequestHandler<DeleteRoleCommand,Unit>
{
    private readonly IAppDbContext _context;
    private readonly IRoleManager _roleManager;

    public DeleteRoleHandler(IAppDbContext context,IRoleManager roleManager)
    {
        _context = context;
        _roleManager = roleManager;
    }
    public async Task<Unit> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await _context.Roles
            .FirstOrDefaultAsync(f => f.Id == request.RoleId, cancellationToken);

        if (role is null)
        {
            throw new NotFoundException(nameof(role));
        }

        _context.Roles.Remove(role);
        await _context.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}