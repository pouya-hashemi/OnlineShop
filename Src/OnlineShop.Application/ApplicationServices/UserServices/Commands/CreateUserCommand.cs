using MediatR;
using OnlineShop.Application.ApplicationServices.UserServices.Responses;
using OnlineShop.Domain.DomainServices;
using OnlineShop.Domain.Interfaces;
using OnlineShop.Domain.Interfaces.DomainServiceInterfaces;
using Mapster;

namespace OnlineShop.Application.ApplicationServices.UserServices.Commands;

public class CreateUserCommand:IRequest<UserDto>
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string PasswordReEnter { get; set; }
    public string UserTitle { get; set; }
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
        var user = await _userManager.CreateUserAsync(request.Username, request.Password, request.PasswordReEnter,
            request.UserTitle,cancellationToken);

        _context.Users.Add(user);

        await _context.SaveChangesAsync(cancellationToken);

        var userDto = user.Adapt<UserDto>();
        return userDto;
    }
}