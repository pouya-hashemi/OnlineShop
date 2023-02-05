using MediatR;
using Microsoft.AspNetCore.Identity;
using OnlineShop.Domain.Entities;
using OnlineShop.Domain.Exceptions.BaseExceptions;
using OnlineShop.Domain.Interfaces.DomainServiceInterfaces;

namespace OnlineShop.Application.ApplicationServices.UserServices.Commands;

public class LoginUserCommand:IRequest<string>
{
    public string UserName { get; set; }
    public string Password { get; set; }
    
}
public class LoginUserHandler:IRequestHandler<LoginUserCommand,string>
{
    private readonly UserManager<User> _userManager;
    private readonly ITokenManager _tokenManager;

    public LoginUserHandler(UserManager<User> userManager,ITokenManager tokenManager)
    {
        _userManager = userManager;
        _tokenManager = tokenManager;
    }
    public async Task<string> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(request.UserName);
        if (user is null)
        {
            throw new NotFoundException(nameof(user));
        }

        if (await _userManager.CheckPasswordAsync(user,request.Password))
        {
            throw new BadRequestException("your password is wrong.");
        }

        var userRoles = await _userManager.GetRolesAsync(user);
        if (userRoles.Any(a=>a.ToLower()=="vendor"))
        {
            return _tokenManager.GenerateVendorToken(user);

        }

        return _tokenManager.GenerateGuestToken();

    }
}