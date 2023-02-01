using MediatR;
using OnlineShop.Application.ApplicationServices.UserServices.Responses;
using OnlineShop.Domain.DomainServices;
using OnlineShop.Domain.Interfaces;
using OnlineShop.Domain.Interfaces.DomainServiceInterfaces;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Domain.Entities;

namespace OnlineShop.Application.ApplicationServices.UserServices.Commands;

public class CreateUserCommand:IRequest<UserDto>
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string PasswordReEnter { get; set; }
    public string UserTitle { get; set; }
    public IEnumerable<int> RoleIds { get; set; }
}
public class CreateUserHandler:IRequestHandler<CreateUserCommand,UserDto>
{
    private readonly IUserManager _userManager;
    private readonly IAppDbContext _context;

    public CreateUserHandler(IUserManager userManager,IAppDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }
    public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var roles = new List<IdentityUserRole<long>>();
        
        var user = await _userManager.CreateUserAsync(request.Username, request.Password, request.PasswordReEnter,
            request.UserTitle,cancellationToken);
        if (request.RoleIds != null)
        {
            roles = await _context.Roles
                .Where(w => request.RoleIds.Any(a => w.Id == a))
                .Select(s=>new IdentityUserRole<long>()
                {
                    UserId = user.Id,
                    RoleId = s.Id
                })
                .ToListAsync(cancellationToken);
        }


        await _context.SaveChangesAsync(cancellationToken);


        _context.UserRoles.AddRange(roles);

        await _context.SaveChangesAsync(cancellationToken);

        var userDto = user.Adapt<UserDto>();
        return userDto;
    }
}