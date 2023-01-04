using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Domain.Exceptions.BaseExceptions;
using OnlineShop.Domain.Interfaces;
using OnlineShop.Domain.Interfaces.DomainServiceInterfaces;

namespace OnlineShop.Application.ApplicationServices.UserServices.Commands;

public class DeleteUserCommand : IRequest
{
    public long UserId { get; set; }
}

public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, Unit>
{
    private readonly IAppDbContext _context;
    private readonly IUserManager _userManager;

    public DeleteUserHandler(IAppDbContext context,
        IUserManager userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(f => f.Id == request.UserId, cancellationToken);

        if (user is null)
        {
            throw new NotFoundException(nameof(user));
        }

        if (_userManager.IsDeletable(user))
        {
            _context.Users.Remove(user);

            await _context.SaveChangesAsync(cancellationToken);
        }


        return Unit.Value;
    }
}