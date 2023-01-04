using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Domain.DomainServices;
using OnlineShop.Domain.Exceptions.BaseExceptions;
using OnlineShop.Domain.Interfaces;
using OnlineShop.Domain.Interfaces.DomainServiceInterfaces;

namespace OnlineShop.Application.ApplicationServices.UserServices.Commands;

public class UpdateUserCommand : IRequest
{
    public long UserId { get; set; }
    public string Username { get; set; }
    public string UserTitle { get; set; }
}

public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, Unit>
{
    private readonly IAppDbContext _context;
    private readonly IUserManager _userManager;

    public UpdateUserHandler(IAppDbContext context,
        IUserManager userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(f => f.Id == request.UserId, cancellationToken);

        if (user is null)
        {
            throw new NotFoundException(nameof(user));
        }

        await _userManager.ChangeUsernameAsync(user, request.Username);
        _userManager.ChangeUserTitle(user, request.UserTitle);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}