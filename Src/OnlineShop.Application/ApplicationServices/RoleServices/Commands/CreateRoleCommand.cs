using Mapster;
using MediatR;
using OnlineShop.Application.ApplicationServices.RoleServices.Responses;
using OnlineShop.Domain.Interfaces;
using OnlineShop.Domain.Interfaces.DomainServiceInterfaces;

namespace OnlineShop.Application.ApplicationServices.RoleServices.Commands;

public class CreateRoleCommand:IRequest<RoleDto>
{
    public string Name { get; set; }
}
public class CreateRoleHandler:IRequestHandler<CreateRoleCommand,RoleDto>
{
    private readonly IRoleManager _roleManager;
    private readonly IAppDbContext _context;

    public CreateRoleHandler(IRoleManager roleManager,IAppDbContext context)
    {
        _roleManager = roleManager;
        _context = context;
    }
    public async Task<RoleDto> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await _roleManager.CreateRoleAsync(request.Name);
        _context.Roles.Add(role);
        await _context.SaveChangesAsync(cancellationToken);

        return role.Adapt<RoleDto>();
    }
}